using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using gprotocol;

public class LoginBonusPanel : BasePanel {

    #region ref

    private const int MAXLOGINDAYS = 7;
    private References ref_LoginBonusPanel;
    private GameObject closebtn;
    private GameObject CloseMask;
    private Dictionary<int,LoginBonusItem> LoginBonusItemList = new Dictionary<int,LoginBonusItem>();
    private static LoginBonusPanel _instance;
    private Action CloseCallBack;
    #endregion

    public static LoginBonusPanel Instance
    {
        get
        {
            if(_instance==null)
            {
                _instance = UIManager.ChangePanel<LoginBonusPanel>("LoginBonusPanel", UIManager.PopPanelRoot);
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }
    private void Awake()
    {
        panelname = "LoginBonusPanel";
        ref_LoginBonusPanel = GetComponent<References>();
        if(ref_LoginBonusPanel!=null)
        {
            closebtn = ref_LoginBonusPanel.Object[0];
            CloseMask = ref_LoginBonusPanel.Object[1];
            closebtn.AddComponent<ButtonClickListener>().onClick = ClosePanel;
            CloseMask.AddComponent<ButtonClickListener>().onClick = ClosePanel;
            for (int step = 1;step<=PlayerManager.BonusList.Count;step++)
            {
                LoginBonusItem item = ref_LoginBonusPanel.Object[1 + step].AddComponent<LoginBonusItem>();
                if(item!=null)
                {
                    LoginBonusItemList.Add(step, item);
                    item.SetInfo(PlayerManager.BonusList[step-1]);
                    item.gameObject.AddComponent<ButtonClickListener>().onClick = GetSigned;
                }
            }
        }
    }
    // Use this for initialization
    void Start () {
		
	}
    #region public

    public override void Close()
    {
        base.Close();
        if(CloseCallBack!= null)
        {
            CloseCallBack();
        }
        if (UIManager.DestroyPanel.Contains(panelname))
        {
            Instance = null;
        }
    }

    public static void Show(Action CloseCallBack = null)
    {
        Instance.Open();
        Instance.CloseCallBack = CloseCallBack;
    }

    public override void Open()
    {
        base.Open();
    }
    /// <summary>
    /// 刷新奖励状态
    /// </summary>
    /// <param name="data"></param>
    public void UpdatePanel(List<LoginBonusData> data)
    {
        for (int step = 1; step <= PlayerManager.BonusList.Count; step++)
        {
            if(PlayerManager.BonusList[step-1].day==data[step-1].day&& PlayerManager.BonusList[step - 1].status!=data[step-1].status)
            {
                PlayerManager.BonusList[step - 1].status = data[step - 1].status;
                LoginBonusItemList[step].SetInfo(PlayerManager.BonusList[step - 1]);
            }
        }
    }

    #endregion

    #region private

    private void ClosePanel(GameObject go)
    {
        Close();
    }
    /// <summary>
    /// 获取登录奖励
    /// </summary>
    /// <param name="go"></param>
    private void GetSigned(GameObject go)
    {
        LoginBonusItem item = go.GetComponent<LoginBonusItem>();
        if(item!=null)
        {
            GetLoginBonusReq msg = new GetLoginBonusReq();
            msg.signtype = (int)item.signtype;
            msg.day = item.DayIndex;
            TcpNet.Instance.send_proto_msg_to_client((int)Stype.System, (int)Cmd.eGetLoginBonusReq, msg);
        }

    }

    #endregion
}
