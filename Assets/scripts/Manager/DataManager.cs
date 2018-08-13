using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager> {

    public float PlayerCarMaxSpeed
    {
        get
        {
            return CarConfig.GetCarMaxSpeed(PlayerCarNumber);
        }
    }
    public float PlayerCarAcceSpeed
    {
        get
        {
            return CarConfig.GetCarAccelerateSpeed(PlayerCarNumber);//玩家车辆加速度
        }
    }
    public float PlayerCarDeceSpeed
    {
        get
        {
            return CarConfig.GetCarDecelerationSpeed(PlayerCarNumber);//玩家车辆减速度
        }
    }
    public float PlayerCarMaxMotorTorque
    {
        get
        {
            return CarConfig.GetCarMaxMotorTorque(PlayerCarNumber);//玩家车辆加速度
        }
    }

    public float PlayerCarShootSpeed
    {
        get
        {
            return CarConfig.GetCarShootSpeed(PlayerCarNumber);//玩家车辆的冲刺速度
        }
    }
    private int PlayerCarNumber;

    public void SetPlayerInfo()
    {
        PlayerCarNumber = 0;
    }
    public string ResURL
    {
        get
        {
            return "http://192.168.96.154/res1/";
        }
    }
    public int PlayerPassWordMaxLength = 14;
    public int PlayerPassWordMinLength = 9; 
    public bool IsFirstEnterGame
    {
        get
        {
            return false;
            return PlayerPrefs.GetString("FirstEnterGame", "")==""?true:false;
        }
    }
    public string ukey
    {
        set
        {
            PlayerPrefs.SetString("PlayerGuestKey", value);
        }
        get
        {
            return PlayerPrefs.GetString("PlayerGuestKey", "");
        }
    }

    public bool is_guest
    {
        set
        {
            if (value == false)
                PlayerPrefs.SetInt("PlayerIsGuest", 0);
            else
                PlayerPrefs.SetInt("PlayerIsGuest", 1);
        }
        get
        {
            return PlayerPrefs.GetInt("PlayerIsGuest",1)==1?true:false;
        }
    }

    public string PlayerAccount
    {
        get
        {
            return PlayerPrefs.GetString("playeraccount", "");
        }
        set
        {
            PlayerPrefs.SetString("playeraccount",value);
        }
    }
    
    public string PlayerPassWord
    {
        get
        {
            return PlayerPrefs.GetString("PlayerPassword", "");
        }
        set
        {
            PlayerPrefs.SetString("PlayerPassword", value);
        }
    }
}
