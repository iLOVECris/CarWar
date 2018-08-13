using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using gprotocol;

public class PlayerAccountUpgradePanel : BasePanel
{
    private GameObject SureBtn;
    private Text SureBtnText;
    private InputField accountinput;
    private InputField passwordinput;
    References ref_playeraccountupgrade;
    private static PlayerAccountUpgradePanel _instance = null;
    public static PlayerAccountUpgradePanel Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UIManager.ChangePanel<PlayerAccountUpgradePanel>("PlayerAccountUpgradePanel", UIManager.BottomPanelRoot);
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }
    // Use this for initialization
    private void Awake()
    {
        panelname = "PlayerAccountUpgrade";
        ref_playeraccountupgrade = GetComponent<References>();
        if(ref_playeraccountupgrade!=null)
        {
            ref_playeraccountupgrade.Object[0].AddComponent<ButtonClickListener>().onClick = ClosePanel;
            ref_playeraccountupgrade.Object[1].AddComponent<ButtonClickListener>().onClick = ClosePanel;
            accountinput = ref_playeraccountupgrade.Object[2].GetComponentInChildren<InputField>();
            passwordinput = ref_playeraccountupgrade.Object[3].GetComponentInChildren<InputField>();
            SureBtn = ref_playeraccountupgrade.Object[4];
            if(SureBtn!=null)
            {
                SureBtnText = SureBtn.GetComponentInChildren<Text>();
                if(SureBtnText!=null)
                {
                    SureBtn.AddComponent<ButtonClickListener>().onClick = SureAccountUpgrade;
                }
            }
        }
    }
    void Start () {
		
	}
    private void ClosePanel(GameObject go)
    {
        Close();
    }
    public override void Close()
    {
        base.Close();
        if (UIManager.DestroyPanel.Contains(panelname))
        {
            Instance = null;
        }
        UserInfoPanel.Instance.UpdatePanel();
    }

    public override void Open()
    {
        base.Open();
    }

    public static void Show()
    {
        Instance.Open();
    }
    private void SureAccountUpgrade(GameObject go)
    {
        if(passwordinput.text.Length==0|| accountinput.text.Length==0)
        {
            PopItem item = UIManager.AddItem<PopItem>("PopItem", UIManager.PopPanelRoot);
            item.SetTips("输入不能为空");
            return;
        }
        if(passwordinput.text.Length>DataManager.Instance.PlayerPassWordMaxLength||passwordinput.text.Length<DataManager.Instance.PlayerPassWordMinLength)
        {
            PopItem item = UIManager.AddItem<PopItem>("PopItem", UIManager.PopPanelRoot);
            item.SetTips("输入长度不符合");
            return;
        }
        if(accountinput.text.Length!=11)
        {
            PopItem item = UIManager.AddItem<PopItem>("PopItem", UIManager.PopPanelRoot);
            item.SetTips("输入手机号不符合");
            return;
        }
        AccountUpgradeReq req = new AccountUpgradeReq();
        req.phonenumber = accountinput.text;
        req.password = utils.GenMd5(passwordinput.text);
        TcpNet.Instance.send_proto_msg_to_client((int)Stype.Auth, (int)Cmd.eAccountUpgradeReq, req);
    }
}
