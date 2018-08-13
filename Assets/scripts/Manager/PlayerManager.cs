using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gprotocol;

public class PlayerManager : Singleton<PlayerManager> {

    public static GuestLoginInfo info;
    public static PlayerData player_data;
    public static string LastPlayerName;
    public static int LastPlayerFaceIcon;
    public static List<LoginBonusData> BonusList;//每日登录数据

}
