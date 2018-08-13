using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using gprotocol;

public class MainUIPanel : BasePanel {

    // Use this for initialization
    private GameObject Root3D;
    private GameObject BottomUIRoot;
    private GameObject TopUIRoot;
    private GameObject SecondUIRoot;
    private References BottomRef;
    private References TopRef;
    private References SecondRef;
    #region ref
    private GameObject friend_btn;
    private GameObject invite_btn;
    private GameObject mession_btn;
    private GameObject activity_btn;
    private GameObject repository_btn;
    private GameObject rank_btn;
    private GameObject setting_btn;
    private GameObject startgame_btn;
    private GameObject playerIconObj;
    private RawImage player_icon;
    private Text player_grade;
    private Text player_name;
    private Text player_gold;
    private static MainUIPanel _instance;
    public static MainUIPanel Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UIManager.ChangePanel<MainUIPanel>("MainUIPanel", UIManager.BottomPanelRoot);                
            }
            return _instance;
        }
    }
    #endregion
    private void Awake()
    {
        panelname = "MainUIPanel";
        Root3D = GameObject.Find("3DRoot");
        BottomUIRoot = transform.FindChild("BottomUIRoot").gameObject;
        if (BottomUIRoot!=null)
        {
            BottomRef = BottomUIRoot.GetComponent<References>();
            if(BottomRef!=null)
            {
                friend_btn = BottomRef.Object[0];
                if(friend_btn!=null)
                    friend_btn.AddComponent<ButtonClickListener>().onClick = ClickFriend;
                invite_btn = BottomRef.Object[1];
                if(invite_btn!=null)
                    invite_btn.AddComponent<ButtonClickListener>().onClick = ClickInvite;
                activity_btn = BottomRef.Object[3];
                if(activity_btn!=null)
                    activity_btn.AddComponent<ButtonClickListener>().onClick = ClickActivity;
                mession_btn = BottomRef.Object[4];
                if(mession_btn!=null)
                    mession_btn.AddComponent<ButtonClickListener>().onClick = ClickMession;
            }
        }
        TopUIRoot = transform.FindChild("TopUIRoot").gameObject;
        if(TopUIRoot!=null)
        {
            TopRef = TopUIRoot.GetComponent<References>();
            if(TopRef!=null)
            {
                repository_btn = TopRef.Object[0];
                if (repository_btn != null)
                    repository_btn.AddComponent<ButtonClickListener>().onClick = ClickRepository;
                rank_btn = TopRef.Object[1];
                if (rank_btn != null)
                    rank_btn.AddComponent<ButtonClickListener>().onClick = ClickRank;
                setting_btn = TopRef.Object[2];
                if (setting_btn != null)
                    setting_btn.AddComponent<ButtonClickListener>().onClick = ClickSetting;
                startgame_btn = TopRef.Object[3];
                if (startgame_btn != null)
                    startgame_btn.AddComponent<ButtonClickListener>().onClick = ClickStartGame;
            }
        }
        SecondUIRoot = transform.FindChild("SecondUIRoot").gameObject;
        if(SecondUIRoot!=null)
        {
            SecondRef = SecondUIRoot.GetComponent<References>();
            if(SecondRef!=null)
            {
                player_grade = SecondRef.Object[1].GetComponent<Text>();
                player_name = SecondRef.Object[2].GetComponent<Text>();
                player_gold = SecondRef.Object[3].GetComponent<Text>();
                playerIconObj = SecondRef.Object[0];
                if(playerIconObj != null)
                {
                    player_icon = playerIconObj.GetComponent<RawImage>();
                    playerIconObj.AddComponent<ButtonClickListener>().onClick = ClickPlayerImage;
                }
            }
        }
    }
    public static void Show()
    {
        Instance.Open();
    }
    public override void Open()
    {
        base.Open();
        UpdatePanel();
    }
    public void UpdatePanel()
    {
        updateSecondUI();
    }

    private void updateSecondUI()
    {
        GuestLoginInfo res = PlayerManager.info;
        PlayerData data = PlayerManager.player_data;
        player_grade.text = data.grade.ToString();
        player_gold.text = data.money.ToString();
        player_name.text = res.name.ToString();
        Root3D.SetActive(true);
        string icon_name = FaceIconConfig.GetResNameById(res.face);
        player_icon.texture = ResManager.GetResource<Texture>(icon_name);
    }
    void ClickFriend(GameObject go)
    {
        PopItem item = null;
        if (UIManager.PopItemPool.Count!=0)//缓存池中有
        {
            item = UIManager.PopItemPool.Dequeue();
            item.transform.localPosition = Vector3.zero;
            item.gameObject.SetActive(true);
        }
        else
        {
            item = UIManager.AddItem<PopItem>("PopItem", UIManager.PopPanelRoot);
        }   
        item.SetTips("clickfriendbtn");
    }
    void ClickInvite(GameObject go)
    {

    }
    void ClickActivity(GameObject go)
    {

    }
    void ClickMession(GameObject go)
    {

    }
    void ClickRepository(GameObject go)
    {

    }
    void ClickRank(GameObject go)
    {

    }
    void ClickSetting(GameObject go)
    {

    }
    void ClickStartGame(GameObject go)
    {

    }
    void ClickPlayerImage(GameObject go)
    {
        UserInfoPanel.Show();
    }
}
