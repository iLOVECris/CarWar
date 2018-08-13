//--------------------------------------------
// Bundle Manager
// Copyright © 2012-2013 GameSeed
//--------------------------------------------
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Collections;
using Compress.LZMAFile;
using Debug_NameSpace;

public class GSBundleManager
{
    const string ABRootPath = "Assets/NewArts/Bundle";
    const string ABsuffix = ".assetbundle";
    public static BuildTarget Platform
    {
        get
        {
#if UNITY_STANDALONE_WIN
            return BuildTarget.StandaloneWindows64;
#elif UNITY_ANDROID
            return BuildTarget.Android;
#elif UNITY_IOS
            return BuildTarget.iOS;
#endif
        }
    }

    public static string PlatformPath
    {
        get
        {
#if UNITY_STANDALONE_WIN
            return "win";
#elif UNITY_ANDROID
            return "android";
#elif UNITY_IOS
            return "ios";
#endif
        }
    }


    [MenuItem("AssetBundleTool/BuildAssetBundle", false, 50)]
    static void CreateAssetBundle()
    {
        SetAllABName();
        string path = EditorUtility.SaveFilePanel("Export AssetBundle", "", "assetbundle", "assetbundle");
        if(string.IsNullOrEmpty(path))
        {
            return;
        }
        int index = path.LastIndexOf("/");
        path = path.Substring(0, index);
        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, Platform);
        AssetDatabase.Refresh();

        Log_Debug.Log("打包OK");
    }

    static void SetAllABName()
    {
        string[] subfolder = AssetDatabase.GetSubFolders(ABRootPath); //Assets/NewArts/Bundle/Config
        for (int index = 0; index < subfolder.Length; index++)
        {
            string abname = subfolder[index].Substring(subfolder[index].LastIndexOf("/") + 1).ToLower() + ABsuffix;
            SetFolderABName(subfolder[index], abname);
        }
    }

    static void SetFolderABName(string root,string abname)
    {
        string[] s = Directory.GetFiles(root);//Assets/NewArts/Bundle/Config\CarConfig.xml

        for (int index = 0; index < s.Length; index++)
        {
            if (!s[index].EndsWith(".meta"))
            {
                AssetImporter importer = AssetImporter.GetAtPath(s[index]);
                if(importer&& !importer.assetBundleName.Equals(abname))
                    importer.assetBundleName = abname;
            }
        }
        string[] sub = Directory.GetDirectories(root);
        for(int i = 0;i<sub.Length;i++)
        {
            SetFolderABName(sub[i], abname);
        }
    }
}