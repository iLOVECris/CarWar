using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Debug_NameSpace;
using gprotocol;
using UnityEngine.SceneManagement;

public partial class NetWorkTcp : Singleton<NetWorkTcp> {
    static DataManager data;

    public void init()
    {
        TcpNet.Instance.RegisterServiceHandler((int)Stype.Auth, AuthServiceEventHandler);
        TcpNet.Instance.RegisterServiceHandler((int)Stype.System, SystemServiceEventHandler);
        RegisterAuthServiceCmdHandler();
        RegisterSystemServiceCmdHandler();
        data = DataManager.Instance;
    }

    #region login
    public void GuestLogin()
    {
        GuestLogin _msg = new GuestLogin();
        string key = DataManager.Instance.ukey;
        if (string.IsNullOrEmpty(key))
        {
            _msg.u_key = utils.rand_str(32);
        }
        else
        {
            _msg.u_key = key;
        }
        DataManager.Instance.ukey = _msg.u_key;
        TcpNet.Instance.send_proto_msg_to_client((int)Stype.Auth, (int)Cmd.eGuestLogin, _msg);
    }

    public void UserLogin()
    {
        UserLoginReq _msg = new UserLoginReq();
        _msg.playeraccount = data.PlayerAccount;
        _msg.password = data.PlayerPassWord;
        TcpNet.Instance.send_proto_msg_to_client((int)Stype.Auth, (int)Cmd.eUserLoginReq, _msg);
    }
    #endregion
}
