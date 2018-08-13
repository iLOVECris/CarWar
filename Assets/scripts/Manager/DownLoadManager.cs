using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using Debug_NameSpace;


public class WWWTask
{
    public WWW www;
    public DownLoadTask task;
    public WWWTask(DownLoadTask task)
    {
        www = new WWW(task.url);
        this.task = task;
    }
}

public class DownLoadTask
{
    public string url;
    public bool isdown;
    public int udata;
    public float progress;
    public Action<WWW, int> callback;
    public DownLoadTask(string url, bool isdown, Action<WWW, int> callback, int udata)
    {
        this.url = url;
        this.isdown = isdown;
        this.callback = callback;
        this.udata = udata;
        progress = 0.0f;
    }
}

public class DownLoadManager : Singleton<DownLoadManager> {

    public System.Security.Cryptography.SHA1 sha1 = new System.Security.Cryptography.SHA1Managed();
    public string SaveLocalPath
    {
        get
        {
            return null;
        }
    }

    public void ShowLoadingErrTips(string s)
    {
        
    }
    public bool RootHashDiff()
    {
        if(RemoteVersion.RemoteVerGroup.Count!=LocalVersion.LocalVerGroup.Count)
        {
            return true;
        }
        else
        {
            foreach (KeyValuePair<string, VerGroup> kv in RemoteVersion.RemoteVerGroup)
            {
                if (LocalVersion.LocalVerGroup.ContainsKey(kv.Key))
                {
                    if(LocalVersion.LocalVerGroup[kv.Key].hash== kv.Value.hash&& LocalVersion.LocalVerGroup[kv.Key].filecount == kv.Value.filecount)
                    {
                        continue;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
        }

        return false;
    }
    public void BeginDownLoad(string path,Action callback)
    {
        Action CompareDiff = () =>
        {
            foreach (KeyValuePair<string, VerGroup> index in RemoteVersion.RemoteVerGroup)
            {

                if (LocalVersion.LocalVerGroup.ContainsKey(index.Key))//比较hash，size
                {
                    VerGroup local = LocalVersion.LocalVerGroup[index.Key];
                    //if (local.hash != index.Value.hash || local.filecount != index.Value.filecount)
                    //{
                    //    LocalVersion.LocalVerGroup[index.Key] = index.Value;
                    //}
                    foreach (KeyValuePair<string, RootGroup> KV in index.Value.rootlist)
                    {
                        if (local.rootlist.ContainsKey(KV.Key))
                        {
                            RootGroup group = local.rootlist[KV.Key];
                            if (group.hash != KV.Value.hash || group.filesize != KV.Value.filesize)
                            {
                                local.rootlist[KV.Key] = KV.Value;
                                local.rootlist[KV.Key].needdownload = true;
                            }
                        }
                        else
                        {
                            RootGroup g = new RootGroup(KV.Value.FileName, KV.Value.hash, KV.Value.filesize);                          
                            g.needdownload = true;
                            local.rootlist[KV.Key] = g;
                        }
                    }

                }
                else
                {
                    VerGroup newgroup = new VerGroup(index.Value.RootName, index.Value.hash, index.Value.filecount);
                    index.Value.SetAllFilesNeedDownLoad();
                    newgroup.rootlist = index.Value.rootlist;
                    LocalVersion.LocalVerGroup[index.Key] = newgroup;
                }

            }
            //LocalVersion.SaveGroupToLocal("allver.ver.txt");
            //TODO::与本地解析比较，找出需要下载的
            if (callback != null)
            {
                callback();
            }
        };
        Action<WWW,int> AllVerCallBack = (www,udata) =>
        {
            if(www.error!=null||www.text==null)
            {
                Log_Debug.LogError("www download error");
                return;
            }
            string info = www.text;
            string[] splited = info.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if(splited.Length<=1)//数据错误
            {
                ShowLoadingErrTips("服务器下载数据错误");
                return;
            }
            for (int i = 0; i < splited.Length; i++)
            {
                if(i==0)
                {
                    string[] first = splited[0].Split(new string[] { ":" }, StringSplitOptions.None);
                    int.TryParse(first[1], out RemoteVersion.Version);
                }
                else
                {
                    string[] first = splited[i].Split(new string[] { "|" }, StringSplitOptions.None);
                    VerGroup ver = new VerGroup(first[0], first[1], int.Parse(first[2]));
                    RemoteVersion.RemoteVerGroup[first[0]] = ver;
                }
            }
            Debug.Log(RootHashDiff());
            if (RemoteVersion.Version > LocalVersion.Version||RootHashDiff())//
            {
                LocalVersion.Version = RemoteVersion.Version;
                foreach (KeyValuePair<string, VerGroup> group in RemoteVersion.RemoteVerGroup)
                {
                    StringBuilder str = new StringBuilder();
                    str.Append(RemoteVersion.remoteurl).Append("/" + group.Key + ".ver.txt");
                    group.Value.CreateDownLoad(str.ToString(), CompareDiff);

                }
            }
            else
            {
                if (callback != null)
                {
                    callback();
                }
            }

        };
        Action MergeLocal = () =>
        {
            RemoteVersion.AddDownLoadTask(path, AllVerCallBack);
        };
        if(DataManager.Instance.IsFirstEnterGame)//首次进入游戏
        {
            LocalVersion.ClearLocalCache();
            RemoteVersion.AddDownLoadTask(path, AllVerCallBack);
        }
        else
        {
            LocalVersion.ReadLocalAllVer(path, MergeLocal);
        }


    }

    public void AddDownLoadTask(string path, Action<WWW, int> callback,int udata = 0)
    {
        DownLoadTask task = new DownLoadTask(path, false, callback, udata);
        GameEnter.instance.queue.Enqueue(task);
        GameEnter.NeedDownLoadCount++;
    }


}
