using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Debug_NameSpace;
using System.IO;
using System.Text;

public class RootGroup
{
    public string FileName;
    public string hash;
    public int filesize;
    public bool needdownload;
    public RootGroup(string FileName,string hash,int filesize)
    {
        this.FileName = FileName;
        this.filesize = filesize;
        this.hash = hash;
        needdownload = false;
    }
    public void CreateDownLoad(string path,Action<RootGroup> callback)
    {
        Action<WWW, int> load = (www, udata) =>
        {
            if (www.error != null || www.text == null)
            {
                Log_Debug.LogError("download ab error");
                return;
            }
            string hash = Convert.ToBase64String(DownLoadManager.Instance.sha1.ComputeHash(www.bytes));
            int length = www.bytes.Length;
            if (this.hash != hash)
                this.hash = hash;
            if (this.filesize != length)
                this.filesize = length;
            string ab_path = System.IO.Path.Combine(LocalVersion.localcachepath, FileName);
            string RootPath = System.IO.Path.GetDirectoryName(ab_path);
            try
            {
                if(!Directory.Exists(RootPath))
                {
                    Directory.CreateDirectory(RootPath);
                }
            }
            catch(Exception e)
            {
                Log_Debug.LogError(e.ToString());
            }

            using (var s = File.Create(ab_path))
            {
                byte[] b = www.bytes;
                s.Write(b, 0, b.Length);
            }
            if(callback!=null)
            {
                this.needdownload = false;
                callback(this);
            }
        };
        DownLoadManager.Instance.AddDownLoadTask(path, load);
    }
}

public class VerGroup{

    public string RootName;
    public string hash;
    public int filecount;
    public Dictionary<string,RootGroup> rootlist;
    public VerGroup(string RootName,string hash,int filecount)
    {
        this.RootName = RootName;
        this.hash = hash;
        this.filecount = filecount;
        rootlist = new Dictionary<string, RootGroup>();
    }
    public void SetAllFilesNeedDownLoad()
    {
        foreach(KeyValuePair<string,RootGroup>kv in rootlist)
        {
            kv.Value.needdownload = true;
        }
    }

    public void CreateDownLoad(string path,Action callback)
    {
        Action<WWW, int> load = (www, udata) =>
        {
            if (www.error != null || www.text == null)
            {
                Log_Debug.LogError("www download error");
                return;
            }
            string info = www.text;
            string[] splited = info.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (splited.Length <= 1)//数据错误
            {
                DownLoadManager.Instance.ShowLoadingErrTips("服务器下载数据错误");
                return;
            }
            for (int i = 0; i < splited.Length; i++)
            {
                if (i == 0)
                {
                    string[] first = splited[0].Split(new string[] { "ver:", "|FileCount:" }, StringSplitOptions.RemoveEmptyEntries);
                    int ver = int.Parse(first[0]);
                    int filecount = int.Parse(first[1]);
                    string hash = Convert.ToBase64String(DownLoadManager.Instance.sha1.ComputeHash(www.bytes));
                    if (RemoteVersion.Version != ver)
                        RemoteVersion.Version = ver;
                    if (this.filecount != filecount)
                        this.filecount = filecount;
                    if (this.hash != hash)
                        this.hash = hash;
                }
                else
                {
                    string[] first = splited[i].Split(new string[] { "|", "@" }, StringSplitOptions.RemoveEmptyEntries);
                    RootGroup root = new RootGroup(first[0], first[1], int.Parse(first[2]));
                    rootlist[first[0]] = root;
                }
            }

            if(callback!=null)
            {
                callback();
            }
        };
        DownLoadManager.Instance.AddDownLoadTask(path, load);
    }

    public void SaveLocal(string filename)
    {
        string name = filename + ".ver.txt";
        StringBuilder in_write = new StringBuilder();
        in_write.Append("ver:").Append(LocalVersion.Version).Append("|FileCount:").Append(filecount);
        foreach(KeyValuePair<string,RootGroup>kv in rootlist)
        {
            in_write.Append("\r\n").Append(kv.Value.FileName).Append("|").Append(kv.Value.hash).Append("@").Append(kv.Value.filesize);
        }
        string path = System.IO.Path.Combine(LocalVersion.localcachepath, name);
        using (var s = System.IO.File.Create(path))
        {
            byte[] b = System.Text.Encoding.UTF8.GetBytes(in_write.ToString());
            s.Write(b, 0, b.Length);
        }
    }
}
