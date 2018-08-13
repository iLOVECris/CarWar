using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gprotocol;
public class UIManager : MonoBehaviour {

    // Use this for initialization
    public static Dictionary<string, BasePanel> PanelPool = new Dictionary<string, BasePanel>();
    public static List<string> DestroyPanel = new List<string>();
    [HideInInspector]
    public static Transform BottomPanelRoot;
    [HideInInspector]
    public static Transform PopPanelRoot;
    [HideInInspector]
    public static Transform TopPanelRoot;
    [HideInInspector]
    public static Transform CacheRoot;
    [HideInInspector]
    public static GameObject Root3D;
    public static ChatPanel chatpanel;
    public static Queue<PopItem> PopItemPool = new Queue<PopItem>();
    public static UIManager Instance;
    public static string CurrentPanel;
    public static string LastPanel;
    const int UILayer = 5;
    private void Awake()
    {
        Instance = this;
        InitDeletePanel();
        Root3D = GameObject.Find("3DRoot");
        BottomPanelRoot = transform.FindChild ("BottomPanel");
        PopPanelRoot = transform.FindChild("PopPanel");
        TopPanelRoot = transform.FindChild("TopPanel");
        CacheRoot = transform.FindChild("CacheRoot");
        MainLobbyPanel.Show();
        MainUIPanel.Show();
        ChatPanel.Show();
        if(PlayerManager.BonusList.Count>0)
            LoginBonusPanel.Show(MainUIPanel.Instance.UpdatePanel);
        CurrentPanel = "MainLobbyPanel";
        LastPanel = "MainLobbyPanel";
    }
    void InitDeletePanel()
    {
        DestroyPanel.Add("UserInfoPanel");
        DestroyPanel.Add("PlayerAccountUpgradePanel");
        DestroyPanel.Add("PlayerReLogin");
        DestroyPanel.Add("LoginBonusPanel");
        DestroyPanel.Add("LoginBonusRewardPanel");
    }
    public static T ChangePanel<T>(string name,Transform root)where T:Component
    {
        LastPanel = CurrentPanel;
        CurrentPanel = name;
        //if(CurrentPanel.Equals(LastPanel))//TODO:思考增加新的判断
        //{
        //    return null;
        //}
        //Root3D.SetActive(CurrentPanel.Equals("MainLobbyPanel")|| CurrentPanel.Equals("MainUIPanel")|| CurrentPanel.Equals("ChatPanel"));
       if (PanelPool.ContainsKey(name))
        {
            T script = PanelPool[name] as T;
            script.transform.parent = root;
            return script;
        }
        else
        {
            GameObject go = ResManager.GetResource<GameObject>(name);
            if(go!=null)
            {
                GameObject obj = Instantiate(go);
                obj.transform.parent = root;
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                RectTransform rect = obj.GetComponent<RectTransform>();
                rect.offsetMax = Vector2.zero;
                rect.offsetMin = Vector2.zero;
                T script = obj.AddComponent<T>();
                return script;
            }
        }
        return null;
    }

    public static T AddItem<T>(string name, Transform root) where T : Component
    {
        GameObject go = ResManager.GetResource<GameObject>(name);
        if (go != null)
        {
            GameObject obj = Instantiate(go);
            obj.transform.parent = root;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            T script = obj.AddComponent<T>();
            return script;
        }
        return null;
    }

}
