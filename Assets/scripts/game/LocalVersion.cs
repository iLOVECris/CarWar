using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using Debug_NameSpace;

public class LocalVersion{

    public static int Version = 0;
    public static int FileCount = 0;
    public static Dictionary<string, VerGroup> LocalVerGroup = new Dictionary<string, VerGroup>();
    public static string localcachepath = Application.persistentDataPath + "/vercache/" + Entrance.PlatformPath;
    public static void ReadLocalAllVer(string path, Action callback)
    {
        Action<string> ReadRoot = (Path) =>
        {
            StringBuilder rootname = new StringBuilder();
            string readtext = null;
            rootname.Append(localcachepath).Append("/").Append(Path).Append(".ver.txt");
            using (var s = System.IO.File.OpenRead(rootname.ToString()))
            {
                byte[] b = new byte[s.Length];
                s.Read(b, 0, b.Length);
                readtext = System.Text.Encoding.UTF8.GetString(b);
            }
            string[] ArraySplited = readtext.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (ArraySplited.Length <= 1)//数据错误
            {
                DownLoadManager.Instance.ShowLoadingErrTips("读取本地数据错误");
                return;
            }
            for (int i = 0; i < ArraySplited.Length; i++)
            {
                if (i == 0)
                {
                    string[] first = ArraySplited[0].Split(new string[] { "ver:", "|FileCount:" }, StringSplitOptions.RemoveEmptyEntries);
                    int ver = int.Parse(first[0]);
                    int filecount = int.Parse(first[1]);
                    FileCount += filecount;
                    if (filecount != LocalVerGroup[Path].filecount)
                    {                       
                        LocalVerGroup[Path].filecount = filecount;
                    }
                        
                }
                else
                {
                    string[] first = ArraySplited[i].Split(new string[] { "|", "@" }, StringSplitOptions.RemoveEmptyEntries);
                    RootGroup root = new RootGroup(first[0], first[1], int.Parse(first[2]));
                    LocalVerGroup[Path].rootlist[first[0]] = root;
                }
            };
            if (callback != null)
                callback();
        };
        string info = null;
        string localpath = localcachepath + path;
        using (var s = System.IO.File.OpenRead(localpath))
        {
            byte[] b = new byte[s.Length];
            s.Read(b, 0, b.Length);
            info = System.Text.Encoding.UTF8.GetString(b);
        }
        string[] splited = info.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        if (splited.Length <= 1)//数据错误
        {
            DownLoadManager.Instance.ShowLoadingErrTips("本地读取数据错误");
            return;
        }
        for (int i = 0; i < splited.Length; i++)
        {
            if (i == 0)
            {
                string[] first = splited[0].Split(new string[] { ":" }, StringSplitOptions.None);
                int.TryParse(first[1], out Version);
            }
            else
            {
                string[] first = splited[i].Split(new string[] { "|" }, StringSplitOptions.None);
                VerGroup ver = new VerGroup(first[0], first[1], int.Parse(first[2]));
                LocalVerGroup[first[0]] = ver;
            }
        }
        foreach (KeyValuePair<string, VerGroup> group in LocalVerGroup)
        {
            ReadRoot(group.Value.RootName);
        }
    }

    public static void ClearLocalCache()
    {
        try
        {           
            if (Directory.Exists(localcachepath))//存在缓存，删除
            {
                Directory.Delete(localcachepath,true);
            }
        }
        catch(Exception e)
        {
            Log_Debug.LogError(e.ToString());
        }
    }

    public static void SaveGroupToLocal(string filename)
    {
        if(!Directory.Exists(localcachepath))
        {
            try
            {
                Directory.CreateDirectory(localcachepath);
            }
            catch(Exception e)
            {
                Log_Debug.LogError(e.ToString());
            }
        }
        string header = "ver:";
        StringBuilder in_write = new StringBuilder();
        in_write.Append(header).Append(Version.ToString());
        foreach(KeyValuePair<string,VerGroup>kv in RemoteVersion.RemoteVerGroup)
        {
            VerGroup ver = kv.Value;
            in_write.Append("\r\n").Append(ver.RootName).Append("|").Append(ver.hash).Append("|").Append(ver.filecount);
            ver.SaveLocal(ver.RootName);
        }
        string path = System.IO.Path.Combine(localcachepath, filename);
        using (var s = System.IO.File.Create(path))
        {
            byte[] b = Encoding.UTF8.GetBytes(in_write.ToString());
            s.Write(b,0,b.Length);
        }
    }


}

