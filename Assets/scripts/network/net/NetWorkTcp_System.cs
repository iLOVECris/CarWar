using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Debug_NameSpace;
using gprotocol;
using UnityEngine.SceneManagement;

public partial class NetWorkTcp{

    void RegisterSystemServiceCmdHandler()
    {
        messagedata msgdata = new messagedata();
        msgdata.RegisterCmdCB<GetPlayerDataRes>((int)Cmd.eGetPlayerDataRes, OnGetPlayerDataRes);//玩家信息与登录奖励
        msgdata.RegisterCmdCB<GetLoginBonusRes>((int)Cmd.eGetLoginBonusRes, OnGetLoginBonusRes);//添加消息回调
        TcpNet.ServiceDic.Add((int)Stype.System, msgdata);
    }


    void SystemServiceEventHandler(msg_cmd msg)
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

    #region system callback
    public static void OnGetPlayerDataRes(GetPlayerDataRes ret)
    {
        if (ret.errcode == 0)
        {
            if (ret != null)
            {
                PlayerManager.BonusList = ret.bonusdata;
                PlayerManager.player_data = ret.playerdata;
                SceneManager.LoadScene("scene_lobby");
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

    public static void OnGetLoginBonusRes(GetLoginBonusRes ret)
    {
        if (ret.errcode == 0)
        {
            if (ret != null)
            {
                RewardStruct reward = new RewardStruct(ret);
                EquipConfig config = EquipConfig.GetEquipByID(ret.id);
                if(config.IsMoney())
                {
                    PlayerManager.player_data.money += ret.num;
                }
                List<RewardStruct> list = new List<RewardStruct>();
                list.Add(reward);
                LoginBonusRewardPanel.Show(list);
                if(LoginBonusPanel.Instance!=null)
                {
                    LoginBonusPanel.Instance.UpdatePanel(ret.bonusdata);
                }
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
    #endregion system callback
}
