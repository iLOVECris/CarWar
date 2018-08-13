using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Debug_NameSpace;
using gprotocol;
using UnityEngine.SceneManagement;

public partial class NetWorkTcp{
    void RegisterAuthServiceCmdHandler()
    {
        messagedata msgdata = new messagedata();
        msgdata.RegisterCmdCB<GuestLoginRes>((int)Cmd.eGuestLoginRes, OnGuestLoginRes);//玩家账号信息
        msgdata.RegisterCmdCB<Respose>((int)Cmd.eRespose, OnModifyRes);//修改名字回调
        msgdata.RegisterCmdCB<AccountUpgradeRes>((int)Cmd.eAccountUpgradeRes, OnAccountUpgradeRes);//游客账号升级回调
        msgdata.RegisterCmdCB<UserLoginRes>((int)Cmd.eUserLoginRes, PlayerAccountLogin);//玩家登录
        msgdata.RegisterCmdCB<UserLoginOutRes>((int)Cmd.eUserLoginOutRes, PlayerLoginOut);//玩家账号注销
        TcpNet.ServiceDic.Add((int)Stype.Auth, msgdata);
    }
    void AuthServiceEventHandler(msg_cmd msg)
    {
        if (TcpNet.Instance.Protocol_type == Protocol_Type.protocol_protobuf)
        {
            MsgCallBack call = TcpNet.ServiceDic[msg.stype].MsgCallBackDic[msg.ctype];
            object value = null;
            try
            {
                value = DecodeCmd.Deserialize(msg.body, call.msg);
            }
            catch (Exception e)
            {
                Log_Debug.LogError("序列化消息失败");
            }
            call.callback.Method.Invoke(null, new object[] { value });
        }
        else
        {
            string cmd = System.Text.Encoding.UTF8.GetString(msg.body);
        }
    }

    #region auth callback
    public static void OnGuestLoginRes(GuestLoginRes ret)
    {
        if (ret.errcode == 0)
        {
            PlayerManager.info = ret.info;
            TcpNet.Instance.send_proto_msg_to_client((int)Stype.System, (int)Cmd.eGetPlayerDataReq, null);//向系统服务器获取数据
        }
        else
        {
            string ErrTips = ErrCodeConfig.GetTipsById(ret.errcode);
            PopItem item = UIManager.AddItem<PopItem>("PopItem", UIManager.PopPanelRoot);
            if (!string.IsNullOrEmpty(ErrTips))
                item.SetTips(ErrTips);
        }
    }
    public static void OnModifyRes(Respose ret)
    {
        string ErrTips = ErrCodeConfig.GetTipsById(ret.status);
        if (!string.IsNullOrEmpty(ErrTips))
        {
            PopItem item = UIManager.AddItem<PopItem>("PopItem", UIManager.PopPanelRoot);
            item.SetTips(ErrTips);
        }
        if (ret.status == 198)//修改名称成功
        {
            PlayerManager.info.name = PlayerManager.LastPlayerName;
        }
        else if (ret.status == 197)//修改头像成功
        {
            PlayerManager.info.face = PlayerManager.LastPlayerFaceIcon;
        }
    }
    public static void OnAccountUpgradeRes(AccountUpgradeRes res)
    {
        string ErrTips = ErrCodeConfig.GetTipsById(res.status);
        PopItem item = UIManager.AddItem<PopItem>("PopItem", UIManager.PopPanelRoot);
        if (!string.IsNullOrEmpty(ErrTips))
            item.SetTips(ErrTips);
        if(res.status==0)
        {
            data.is_guest = false;
            data.PlayerAccount = res.playeraccount;
            data.PlayerPassWord = res.password;
        }
        PlayerAccountUpgradePanel.Instance.Close();
    }
    public static void PlayerAccountLogin(UserLoginRes ret)
    {
        if (ret.errcode == 0)
        {
            PlayerManager.info = ret.info;
            if (UIManager.Instance == null)
            {
                TcpNet.Instance.send_proto_msg_to_client((int)Stype.System, (int)Cmd.eGetPlayerDataReq, null);//向系统服务器获取数据
            }
            else
            {
                PlayerReLoginPanel.Instance.Close();
            }
        }
        else
        {
            string ErrTips = ErrCodeConfig.GetTipsById(ret.errcode);
            PopItem item = UIManager.AddItem<PopItem>("PopItem", UIManager.PopPanelRoot);
            if (!string.IsNullOrEmpty(ErrTips))
                item.SetTips(ErrTips);
        }
    }

    public static void PlayerLoginOut(UserLoginOutRes ret)
    {
        if(ret.status==0)
        {
            PlayerReLoginPanel.Show();           
        }
        else
        {
            string ErrTips = ErrCodeConfig.GetTipsById(ret.status);
            PopItem item = UIManager.AddItem<PopItem>("PopItem", UIManager.PopPanelRoot);
            if (!string.IsNullOrEmpty(ErrTips))
                item.SetTips(ErrTips);
        }
    }
    #endregion auth callback

}
