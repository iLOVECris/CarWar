using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MsgCallBack
{
    public Type msg;
    public Delegate callback;
}
public class messagedata
{

    public Dictionary<int, MsgCallBack> MsgCallBackDic = new Dictionary<int, MsgCallBack>();
    public delegate void MsgCall<T>(T t);


    public void RegisterCmdCB<T>(int msg_number, MsgCall<T> call)
    {
        MsgCallBack c = new MsgCallBack();

        c.callback = call;
        c.msg = typeof(T);
        MsgCallBackDic.Add(msg_number, c);
    }
}
