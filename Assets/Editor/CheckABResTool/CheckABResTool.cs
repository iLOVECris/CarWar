using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
public class CheckABResTool {

    [MenuItem("CheckResABTool/SelectWindow")]
    static void OpenWindow()
    {
        EditorWindow.GetWindow(typeof(SelectWindow), false, "Window");
    }
}
public class ParserABFile
{
    string UrlHeader = "file:///";
    public Dictionary<string, UnityEngine.Object> ShowResTables = new Dictionary<string, UnityEngine.Object>();
    public string[] FilterArray = { "UnityEngine.GameObject", "UnityEngine.Texture2D", "UnityEngine.Material","UnityEngine.Sprite" };
    UnityEngine.Object[] objArray = null;

    public int _selectedPackageName = -1;
    private int _selectedResType = -1;
    string SelectABName = string.Empty;
    internal int SelectedResType
    {
        get
        {
            return _selectedResType;
        }
        set
        {
            if(_selectedResType!=value)
            {
                _selectedResType = value;
                FilterLoad();
            }
        }
    }
    string OpenPanelRecord
    {
        get
        {
            return PlayerPrefs.GetString("OpenPanelRecord", "");
        }
        set
        {
            PlayerPrefs.SetString("OpenPanelRecord", value);
        }
    }
    public string LoadFile()
    {
        string savePath = EditorUtility.OpenFilePanel("Select AssetBundle", OpenPanelRecord, "");
        OpenPanelRecord = savePath;
        load(UrlHeader + savePath, savePath,LoadedAB);
        return FilterABName(savePath);
    }

    void FilterLoad()
    {
        ShowResTables.Clear();
        for(int i = 0;i<objArray.Length;i++)
        {
            if(objArray[i].GetType().ToString()==FilterArray[_selectedResType])
            {
                if(ShowResTables.ContainsKey(objArray[i].name))
                {
                    Debug.Log("重复包含：" + objArray[i].name);
                }
                else
                {
                    ShowResTables.Add(objArray[i].name, objArray[i]);
                }
            }
        }

    }

    void InstanSelectObj(UnityEngine.Object obj)
    {
        Canvas cavas = GameObject.FindObjectOfType<Canvas>();
        if (cavas == null)
        {
            cavas = new GameObject("Root").AddComponent<Canvas>();
        }
        if (obj!=null)
        {
            GameObject go = UnityEngine.Object.Instantiate(obj) as GameObject;
            go.transform.localScale = Vector3.one;
            go.name = obj.name;
            go.transform.parent = cavas.transform;
        }

    }

    void CreateTexture(UnityEngine.Object obj)
    {
        Canvas cavas = GameObject.FindObjectOfType<Canvas>();
        if (cavas == null)
        {
            cavas = new GameObject("Root").AddComponent<Canvas>();
        }
        GameObject go = new GameObject(obj.name);
        go.transform.parent = cavas.transform;
        RawImage image = go.AddComponent<RawImage>();
        image.texture = obj as Texture;
    }
    void ShowSprite(UnityEngine.Object obj)
    {
        Canvas cavas = GameObject.FindObjectOfType<Canvas>();
        if (cavas == null)
        {
            cavas = new GameObject("Root").AddComponent<Canvas>();
        }
        GameObject go = new GameObject(obj.name);
        go.transform.parent = cavas.transform;
        Image image = go.AddComponent<Image>();//DefaultControls.CreateImage
        image.sprite = obj as Sprite;
    }
    void ShowMaterial(UnityEngine.Object obj)
    {
        Canvas cavas = GameObject.FindObjectOfType<Canvas>();
        if (cavas == null)
        {
            cavas = new GameObject("Root").AddComponent<Canvas>();
        }
        GameObject go = new GameObject(obj.name);
        go.transform.parent = cavas.transform;
        RawImage image = go.AddComponent<RawImage>();
        image.material = obj as Material;
    }

    void LoadedAB(AssetBundle ab,string name)
    {
        if(ab!=null)
        {
            objArray = ab.LoadAllAssets();
        }
        else
        {
            Debug.Log("<color=red>Need Parser AssetBundle is null</color>");
        }
    }

    AssetBundle NeedParserAB = null;
    public ParserABFile()
    {
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
    }

    public void ParserTextAreaText(string text)
    {
        if(text.Length>0)
        {
            string[] resNameArray = text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for(int i = 0;i< resNameArray.Length;i++)
            {
                if(ShowResTables.ContainsKey(resNameArray[i]))
                {
                    ShowObj(ShowResTables[resNameArray[i]]);
                }
                else
                {
                    Debug.Log("<color=red>do not have " + resNameArray[i] + "</color>");
                }
            }

        }
    }

    void ShowObj(UnityEngine.Object obj)
    {
        if(_selectedResType==0)
        {
            InstanSelectObj(obj);
        }
        else if(_selectedResType==1)
        {
            CreateTexture(obj);
        }
        else if(_selectedResType==2)
        {
            ShowMaterial(obj);
        }
        else if(_selectedResType==3)
        {
            ShowSprite(obj);
        }
    }

    public void load(string DownloadPath,string FilePath,Action<AssetBundle,string>Loaded)
    {
        Action<WWW> callback = (www) =>
            {
                Exception _err = null;
                SelectABName = FilterABName(DownloadPath);
                try
                {
                    if(www.error==null&&www.bytes!=null)
                    {
                        NeedParserAB = www.assetBundle;
                        Loaded(NeedParserAB, SelectABName);
                        NeedParserAB.Unload(false);//必须卸载掉，否则WWW再次加载会报错,参数unloadAllLoadedObjects
                        Resources.UnloadUnusedAssets();
                    }
                    else
                    {
                        _err = new Exception();
                    }
                }
                catch(Exception error)
                {
                    _err = error;
                }
                if(_err!=null)
                {
                    Debug.Log("<color=red>download " + SelectABName + " error </color>");
                }
            };
        DownLoad task = DownLoad.New(DownloadPath, callback);
        task.StartDownLoad();
        
    }
    private string FilterABName(string abname)
    {
        string[] nameArray = abname.Split('/');
        return nameArray[nameArray.Length - 1];
    }

    public static void ResetShader(GameObject parent)
    {
        if(parent==null)
        {
            return;
        }
        Renderer[] rlist = parent.GetComponentsInChildren<Renderer>(true);
        foreach(Renderer item in rlist)
        {
            if(item!=null)
            {
                foreach(Material m in item.sharedMaterials)
                {
                    if(m!=null)
                    {
                        m.shader = Shader.Find(m.shader.name);
                    }
                }
            }
        }

    }

}

public class DownLoad:MonoBehaviour
{
    public Action<WWW> callback;
    public string url;
    static GameObject netObject;
    public static DownLoad New(string url,Action<WWW>callback)
    {
        netObject = new GameObject(System.DateTime.Now.ToString("hh:mm:ss"));
        DownLoad download = netObject.AddComponent<DownLoad>();
        download.url = url;
        download.callback = callback;
        return download;
    }

    public void StartDownLoad()
    {
        StartCoroutine(AyscLoading());
    }
    IEnumerator AyscLoading()
    {
        Debug.Log(url);
        WWW www = new WWW(url);
        try
        {
            yield return www;
            if (www.error == null && www.bytes != null)
            {
                Debug.Log("download ok");
                this.callback(www);
                DestroyImmediate(netObject);
            }
        }
        finally
        {
            www.Dispose();
        }
    }

}
