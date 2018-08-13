using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RemoteVersion {

    public static int Version = 0;
    public static readonly string remoteurl = Entrance.ResUrl + Entrance.PlatformPath;

    public static Dictionary<string,VerGroup> RemoteVerGroup = new Dictionary<string,VerGroup>();
    public static void AddDownLoadTask(string path, Action<WWW,int> callback,int udata = 0)
    {
        path = remoteurl + path;
        DownLoadTask task = new DownLoadTask(path, false, callback,udata);
        GameEnter.instance.queue.Enqueue(task);
    }


}
