using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using gprotocol;

public class UserInfoPanel : BasePanel
{
    private Transform UpArea;
    private Transform DownArea;
    private Transform RightArea;
    private References ref_uparea;
    private References ref_downarea;
    private References ref_rightarea;
    private GameObject ModifyPlayerName;
    private InputField PlayerName;
    private GameObject PlayerUpgrade;
    private Text PlayerUpgradeText;
    RawImage PlayerIcon;
    private GameObject FaceIconObj;
    Text PlayerExp;
    Slider ExpSlider;
    Color BtnSelectDefaultColor = new Color32(255, 237, 0, 114);
    Color BtnDefaultColor = new Color32(84, 84, 87, 87);
    #region right
    private Image Playerinfo;
    private Image SelfScore;
    private Image HistroyScore;
    private GameObject CloseBtn;
    private Image LastSelectBtn;
    #endregion
    private static UserInfoPanel _instance = null;
    public static UserInfoPanel Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UIManager.ChangePanel<UserInfoPanel>("UserInfoPanel", UIManager.BottomPanelRoot);
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }
    void Awake()
    {
        panelname = "UserInfoPanel";
        UpArea = this.transform.FindChild("UpArea");
        if(UpArea!=null)
        {
            ref_uparea = UpArea.GetComponent<References>();
            if(ref_uparea!=null)
            {
                ModifyPlayerName = ref_uparea.Object[4];
                PlayerIcon = ref_uparea.Object[0].GetComponent<RawImage>();               
                PlayerName = ref_uparea.Object[1].GetComponent<InputField>();
                PlayerExp = ref_uparea.Object[3].GetComponent<Text>();
                ExpSlider = ref_uparea.Object[2].GetComponent<Slider>();
                FaceIconObj = ref_uparea.Object[5];
                if (FaceIconObj != null)
                {
                    FaceIconObj.AddComponent<ButtonClickListener>().onClick = ChangePlayerIcon;
                }
                if (PlayerName!=null)
                {
                    PlayerManager.LastPlayerName = PlayerName.text;
                    PlayerName.onEndEdit.AddListener(delegate { ModifyName(); });
                }
                PlayerUpgrade = ref_uparea.Object[6];
                if(PlayerUpgrade!=null)
                {
                    PlayerUpgradeText = PlayerUpgrade.GetComponentInChildren<Text>();
                    PlayerUpgradeText.text = DataManager.Instance.is_guest ? "账号升级":"注销账号";
                    PlayerUpgrade.AddComponent<ButtonClickListener>().onClick = PlayerAccountUpgrade;
                }
            }
        }
        DownArea = this.transform.FindChild("DownArea");
        if (DownArea != null)
        {
            ref_downarea = DownArea.GetComponent<References>();
        }
        RightArea = this.transform.FindChild("RightArea");
        if (RightArea != null)
        {
            ref_rightarea = RightArea.GetComponent<References>();
            if(ref_rightarea!=null)
            {
                Playerinfo = ref_rightarea.Object[1].GetComponent<Image>();
                if(Playerinfo!=null)
                {
                    Playerinfo.gameObject.AddComponent<ButtonClickListener>().onClick = RightAreaBtnClick;
                    //LastSelectBtn = Playerinfo;
                }
                SelfScore = ref_rightarea.Object[0].GetComponent<Image>();
                if(SelfScore!=null)
                {
                    SelfScore.gameObject.AddComponent<ButtonClickListener>().onClick = RightAreaBtnClick;
                }
                HistroyScore = ref_rightarea.Object[2].GetComponent<Image>();
                if(HistroyScore!=null)
                {
                    HistroyScore.gameObject.AddComponent<ButtonClickListener>().onClick = RightAreaBtnClick;
                }
                CloseBtn = ref_rightarea.Object[3];
                if(CloseBtn!=null)
                {
                    CloseBtn.AddComponent<ButtonClickListener>().onClick = ClosePanel;
                }
            }
        }

        RightAreaBtnClick(Playerinfo.gameObject);
    }
	// Use this for initialization
	void Start () {
		
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
    void ClosePanel(GameObject go)
    {
        Close();
        MainUIPanel.Instance.UpdatePanel();
    }
    public void UpdatePanel()
    {
        GuestLoginInfo info = PlayerManager.info;
        PlayerName.text = info.name;
        PlayerManager.LastPlayerName = PlayerName.text;
        string icon_name = FaceIconConfig.GetResNameById(info.face);
        PlayerIcon.texture = ResManager.GetResource<Texture>(icon_name);
        PlayerUpgradeText.text = DataManager.Instance.is_guest ? "账号升级" : "注销账号";
        //PlayerUpgrade.gameObject.SetActive(DataManager.Instance.is_guest);
    }
    // Update is called once per frame
    void Update () {
		
	}
    public override void Close()
    {
        base.Close();
        if (UIManager.DestroyPanel.Contains(panelname))
        {
            Instance = null;
        }
    }

    #region 按钮响应

    void ChangePlayerIcon(GameObject go)//修改头像
    {
        ChangePlayerNamePanel.Show();
    }
    void PlayerAccountUpgrade(GameObject go)//玩家游客账号升级
    {
        bool player_is_guest = DataManager.Instance.is_guest;
        if(!player_is_guest)
        {
            TcpNet.Instance.send_proto_msg_to_client((int)Stype.Auth, (int)Cmd.eUserLoginOutReq, null);
        }
        else
        {
            PlayerAccountUpgradePanel.Show();
        }
    }
    void ModifyName()//修改名字
    {
        //TODO::名字合法性检查
        if(PlayerManager.LastPlayerName.Equals(PlayerName.text))
        {
            return;
        }
        ModifyName msg = new gprotocol.ModifyName();
        msg.name = PlayerName.text;
        TcpNet.Instance.send_proto_msg_to_client((int)Stype.Auth, (int)Cmd.eModifyName, msg);
        PlayerManager.LastPlayerName = PlayerName.text;
    }
    void RightAreaBtnClick(GameObject go)
    {
        if(LastSelectBtn!=null)//不是第一次点击
        {
            if (LastSelectBtn.gameObject == go)
            {
                return;
            }
            LastSelectBtn.color = BtnDefaultColor;
        }
        if (go.name.Equals("self"))
        {
            SelfScore.color = BtnSelectDefaultColor;
            LastSelectBtn = SelfScore;
        }
        else if(go.name.Equals("info"))
        {
            Playerinfo.color = BtnSelectDefaultColor;
            LastSelectBtn = Playerinfo;
        }
        else if(go.name.Equals("histroy"))
        {
            HistroyScore.color = BtnSelectDefaultColor;
            LastSelectBtn = HistroyScore;
        }


    }
    #endregion
}
