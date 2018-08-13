using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {
    //不使用WheelCollider的参数(使用controler使用)
	public static float CarAdjustParam = 23.875f;//车子左右调整的参数
    public static float WheelAdjustParam = 23.511f;//轮子左右调整的参数
    public static int DriftTimes = 33;//漂移帧数
    public static int DriftCft = 200;//漂移角度系数
    public static float DriftMinAngle = 0.7887f;//最小漂移度数
    public static float DriftAngleStep = 2.222f;//漂移每步度数


    public static float SmoothTime = 3f;//平滑阻尼
    public static float CameraHight = 5.0f;//摄像机与目标的高度差
    public static float CameraToCarDistanceNormal = 10.0f;//普通跑时摄像机与车的前后距离
    public static float CameraToCarDistanceAcce = 15.0f;//加速时摄像机与车的前后距离

    //使用CarControler使用
    public static float CarMotorTorque = 1500f;//车的wheelcollider力矩
    public static float maxSteerAngle = 30.0f;//最大车旋转角度
    public static float maxSpeedSteerAngle = 10.0f;
    public static float AcclerateTimes = 54;//车冲刺的帧数
    public static float CameraChaseTimes = 15;//摄像机追随帧数

    public static float FadeEaseBase = 0.8f;//汽车缓动开始系数

    public static string CarBaseName = "CarAvt";//汽车起始名称

    public static float CarFadeTime = 1.0f;//汽车淡出时间

    public static float RotateRadio = 0.288f;//车随鼠标旋转系数
    public static int ChatLineMaxSize = 260;//聊天框每行最大字长度
    public static int ChatItemBaseLength = 40;//聊天输入框的基础大小
    public static int ChatItemBaseLineHeight = 30;//聊天输入框的基础高度
    public static int EmojiCount = 45;//聊天表情的数量
    public static int PopItemMove = 65;//popitem上升幅度
    public static float FaceIconWidth = 100;//头像item每个宽度
    public static float FaceIconOneLineCount = 5.0f;//头像一行的个数
    public static int FaceTitleWidth = 30;//头像title的宽度
}
