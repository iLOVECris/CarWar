using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Text;

public class ResManager : MonoBehaviour {

    public static AssetBundle ConfigAssetBundle;
    public static AssetBundle PrefabAssetBundle;
    public static string localcachepath = Application.persistentDataPath + "/vercache/" + Entrance.PlatformPath;
    public static Dictionary<string, AssetBundle> ABDic = new Dictionary<string, AssetBundle>();
    public static List<string> LoadABPath = new List<string>();
    // Use this for initialization
    void Start () {
		
	}
    public static void AssetBundleInit()
    {
        LoadABPath.Add("Root/config.assetbundle");
        LoadABPath.Add("Root/prefabs.assetbundle");
        LoadABPath.Add("Root/ui.assetbundle");
        LoadABPath.Add("Root/Resources.assetbundle");
    }
    static void AssetBundleLoading(string path,string key,Action<string,AssetBundle>a)
    {
        AssetBundle ab = AssetBundle.LoadFromFile(path);
        a(key, ab);
    }
    public static void LoadAssetBundleInMemory(Action<string> callback)
    {
        Action<string, AssetBundle> AddDic = (str, ab) =>
         {
             ABDic[str] = ab;
             if (str.Equals("Root/config.assetbundle"))
                 ConfigAssetBundle = ab;
             callback(str);
         };
        foreach(string abpath in LoadABPath)
        {
            StringBuilder ABPath = new StringBuilder();
            ABPath.Append(localcachepath).Append("/").Append(abpath);
            AssetBundleLoading(ABPath.ToString(), abpath, AddDic);

        }

    }
    public static T GetResource<T>(string name,AssetBundle ab = null) where T : UnityEngine.Object
    {
        if(string.IsNullOrEmpty(name))
        {
            return null;
        }
        if(ab!=null)//先从指定的中找
        {
            T obj = ab.LoadAsset(name,typeof(T)) as T;
            if(ab!=null)
            {
                return obj;
            }
        }
        foreach(KeyValuePair<string,AssetBundle>kv in ABDic)
        {
            if(kv.Value.Contains(name))
            {
                T obj = kv.Value.LoadAsset(name, typeof(T)) as T;
                if (obj != null)
                {
                    return obj;
                }
            }
        }
//#if UNITY_EDITOR
//        T Obj = Resources.Load(name, typeof(T)) as T;
//        if (Obj != null)
//        {
//            return Obj;
//        }
//        return null;

//#endif
        return null;
    }
}
