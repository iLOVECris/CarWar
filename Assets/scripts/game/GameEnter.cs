using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class GameEnter : MonoBehaviour {

    // Use this for initialization
    GameObject Default;
    GameObject Loading;
    GameObject logo;
    Slider progress;
    GameObject GuestLoginBtn;
    public static GameEnter instance;
    public static int AllResCount;
    public static IList<RootGroup> NeedDownLoadFiles;
    public bool BeginDL = false;
    public float nowprogress;
    public Queue<DownLoadTask> queue = new Queue<DownLoadTask>();
    public List<WWWTask> task_list = new List<WWWTask>();
    public int DownLoadOkCount = 0;//下载好的数量
    public static int NeedDownLoadCount = 0;
    public static Transform PopRoot;
    private void Awake()
    {
        instance = this;
        AllResCount = 0;
        PopRoot = this.transform;
        logo = transform.Find("logo").gameObject;
        Loading = transform.Find("Loading").gameObject;
        Default = transform.Find("Default").gameObject;
        progress = transform.Find("Loading/Slider").GetComponent<Slider>();
        GuestLoginBtn = transform.Find("GuestLogin").gameObject;
        NetWorkTcp.Instance.init();
    }


    IEnumerator Start() {
        logo.SetActive(true);
        DOVirtual.DelayedCall(1.2f, delegate ()
        {
            this.logo.transform.DOScale(Vector3.zero, 1).OnComplete(delegate () {
                logo.SetActive(false);
                Default.SetActive(true);
                Loading.SetActive(true);
            });
        }, true);
        yield return new WaitForSeconds(2.5f);
        StartLoading();

    }
   
    void ParseConfigOver()//显示登录
    {
        if(DataManager.Instance.is_guest)
        {
            if (!GuestLoginBtn.activeInHierarchy)
            {
                GuestLoginBtn.SetActive(true);
                if (GuestLoginBtn != null)
                {
                    GuestLoginBtn.AddComponent<ButtonClickListener>().onClick = GuestLoginBtnOnClick;
                }
            }
        }
        else//使用账号密码登录
        {
            NetWorkTcp.Instance.UserLogin();
        }
    }

    void DownLoadAll()
    {
        NeedDownLoadFiles = GetNeedDownLoadFiles() as IList<RootGroup>;
        if(NeedDownLoadFiles.Count>0)
        {
            Debug.Log("have need download files");
            BeginDownLoad();
        }
        else
        {
            SetSliderProgress(1.0f);
            ResManager.AssetBundleInit();
            ResManager.LoadAssetBundleInMemory((str) =>
            {
                if(str.Equals("Root/config.assetbundle"))//解析配置文件
                {
                    ConfigManager.Instance.Parse(ParseConfigOver);
                }
            });
        }

    }
    void DownloadOk(RootGroup g)
    {
        lock(NeedDownLoadFiles)
        {
            NeedDownLoadFiles.Remove(g);
            if(NeedDownLoadFiles.Count==0)
            {
                LocalVersion.SaveGroupToLocal("allver.ver.txt");
                SetSliderProgress(1.0f);
                ResManager.AssetBundleInit();
                ResManager.LoadAssetBundleInMemory((str) =>
                {
                    if (str.Equals("Root/config.assetbundle"))//解析配置文件
                    {
                        ConfigManager.Instance.Parse(ParseConfigOver);
                    }
                });
            }
        }
    }
    void BeginDownLoad()
    {
        BeginDL = true;
        foreach(RootGroup root in NeedDownLoadFiles)
        {
            string path = System.IO.Path.Combine(RemoteVersion.remoteurl, root.FileName);
            root.CreateDownLoad(path,DownloadOk);
        }
    }
    void StartLoading()
    {     
        DownLoadManager.Instance.BeginDownLoad("/allver.ver.txt",DownLoadAll);
    }
    public void GuestLoginBtnOnClick(GameObject go)//游客登录
    {
        NetWorkTcp.Instance.GuestLogin();
    }
    IEnumerable<RootGroup> GetNeedDownLoadFiles()
    {
        List<RootGroup> file_list = new List<RootGroup>();
        foreach (KeyValuePair<string, VerGroup>kv in LocalVersion.LocalVerGroup)
        {
            foreach(KeyValuePair<string,RootGroup>_kv in kv.Value.rootlist)
            {
                if(_kv.Value.needdownload)
                {
                    Debug.Log(_kv.Key);
                    file_list.Add(_kv.Value);
                }
            }
        }
        AllResCount = file_list.Count;
        return file_list;
    }

    public void SetSliderProgress(float value)
    {
        progress.value = value;
    }
    private void LateUpdate()
    {
        if(BeginDL)
        {          
            int progress = (int)(((DownLoadOkCount-2) + nowprogress) * 10 / NeedDownLoadCount * 70);
            Debug.Log((DownLoadOkCount-2) + ".." + nowprogress + ".." + NeedDownLoadCount + ".." + progress);
            progress += 300;
            if (progress>=1000)
            {
                progress = 1000;
            }
            SetSliderProgress(progress*0.01f);
        }

    }
    private void Update()
    {
        lock (task_list)
        {
            if (queue.Count > 0)
            {
                task_list.Add(new WWWTask(queue.Dequeue()));
                nowprogress = 0;
            }
            if (task_list.Count > 0)
            {
                List<WWWTask> Finished = new List<WWWTask>();
                for (int i = 0; i < task_list.Count; i++)
                {
                    WWWTask w = task_list[i];
                    if (w.www.isDone)
                    {
                        Finished.Add(w);
                        w.task.callback(w.www, w.task.udata);
                        w.www.Dispose();
                        DownLoadOkCount++;
                    }
                    else
                    {
                        w.task.progress = w.www.progress;
                        nowprogress = w.www.progress;
                    }
                }

                foreach (WWWTask task in Finished)
                {
                    task_list.Remove(task);
                }
            }

        }
    }
}
