using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Debug_NameSpace;

public class ConfigManager : MonoBehaviour {
    Dictionary<string, Type> ConfigDic = new Dictionary<string, Type>();
    private static ConfigManager _instance;
    public static ConfigManager Instance
    {
        get
        {
            if(_instance==null)
            {
                return new ConfigManager();
            }
            return _instance;
        }
    }
    private void RegType<T>(String name)
    {
        ConfigDic.Add(name, typeof(T));
    }
    void InitConfig()
    {     
        RegType<CarConfig>(CarConfig.urlkey);
        RegType<ErrCodeConfig>(ErrCodeConfig.urlkey);
        RegType<FaceIconConfig>(FaceIconConfig.urlkey);
        RegType<EquipConfig>(EquipConfig.urlkey);
    }
    Action LoadResOk = null;
    public void Parse(Action LoadOk)
    {
        if(LoadOk != null)
        {
            LoadResOk = LoadOk;
        }
        InitConfig();
        int ConfigCount = ConfigDic.Count;
        foreach (KeyValuePair<string, Type>type in ConfigDic)
        {
            TextAsset xmlText = ResManager.GetResource<TextAsset>(type.Key);
            if (xmlText==null)
            {
                Log_Debug.LogError("load config err");
            }
            try
            {
                type.Value.GetMethod("Parse").Invoke(null, new object[1] { xmlText });
            }
           catch(Exception err)
            {
                Log_Debug.Log(type.Key);
                Log_Debug.LogError("loading config error:" + err.Message);   
            }
            ConfigCount--;
            if (ConfigCount<=0)
            {
                if(LoadResOk != null)
                {
                    LoadResOk();
                }
            }
        }
    }


}
