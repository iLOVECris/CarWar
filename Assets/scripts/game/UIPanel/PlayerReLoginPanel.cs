using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using gprotocol;

public class PlayerReLoginPanel : BasePanel
{
    private GameObject SureBtn;
    private InputField accountinput;
    private InputField passwordinput;
    References ref_PlayerReLogin;
    private static PlayerReLoginPanel _instance;
    static DataManager data;
    public static PlayerReLoginPanel Instance
    {
        get
        {
            if(_instance==null)
            {
                _instance = UIManager.ChangePanel<PlayerReLoginPanel>("PlayerReLoginPanel", UIManager.BottomPanelRoot);
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
        panelname = "PlayerReLoginPanel";
        data = DataManager.Instance;
        ref_PlayerReLogin = GetComponent<References>();
        if(ref_PlayerReLogin!=null)
        {
            accountinput = ref_PlayerReLogin.Object[0].GetComponentInChildren<InputField>();
            passwordinput = ref_PlayerReLogin.Object[1].GetComponentInChildren<InputField>();
            SureBtn = ref_PlayerReLogin.Object[2];
            if(SureBtn!=null)
            {
                SureBtn.AddComponent<ButtonClickListener>().onClick = ReLogin;
            }
        }
    }

    public static void Show()
    {
        Instance.Open();
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
    void ReLogin(GameObject go)
    {
        if (passwordinput.text.Length == 0 || accountinput.text.Length == 0)
        {
            PopItem item = UIManager.AddItem<PopItem>("PopItem", UIManager.PopPanelRoot);
            item.SetTips("输入不能为空");
            return;
        }
        if (passwordinput.text.Length > DataManager.Instance.PlayerPassWordMaxLength || passwordinput.text.Length < DataManager.Instance.PlayerPassWordMinLength)
        {
            PopItem item = UIManager.AddItem<PopItem>("PopItem", UIManager.PopPanelRoot);
            item.SetTips("输入长度不符合");
            return;
        }
        if (accountinput.text.Length != 11)
        {
            PopItem item = UIManager.AddItem<PopItem>("PopItem", UIManager.PopPanelRoot);
            item.SetTips("输入手机号不符合");
            return;
        }
        UserLoginReq _msg = new UserLoginReq();
        _msg.playeraccount = accountinput.text;
        _msg.password = utils.GenMd5(passwordinput.text);
        data.PlayerAccount = _msg.playeraccount;
        data.PlayerPassWord = _msg.password;
        TcpNet.Instance.send_proto_msg_to_client((int)Stype.Auth, (int)Cmd.eUserLoginReq, _msg);
    }



}
