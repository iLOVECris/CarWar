using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public enum CarStatusType
{
    LineRun = 0,
    LeftTurn,
    RightTurn,
}

public class CarControler : MonoBehaviour {

    private const int WHEEL_COUNT = 2;
    private bool isCanTurn;//是否是一直转
    private CarStatusType carstatus;//车的状态
    private bool DriftCar;//漂移
    private bool CanDrift;//是否可以漂移
    private bool AccelerateCar;//加速
    private float currentspeed = 0.0f;//当前的速度
    int DriftIndex;//漂移帧数
    Vector3 Forward;//漂移前的速度方向
    private float DriftBaseTime;//漂移前的时间
    private float SpeedTemp;//漂移前的速度
    References r;
    private Transform MainCamera;
    Rigidbody rigidbody;
    public WheelCollider flWheelCollider;
    public WheelCollider frWheelCollider;
    public WheelCollider rlWheelCollider;
    public WheelCollider rrWheelCollider;

    public static bool IsAccelerate;//是否加速
    public static bool IsAccelerateOver;//加速完毕
    private float carMaxSpeed;//车辆的最大速度
    private float carAccSpeed;//车辆加速度
    private float carDeSpeed;//车辆减速度
    private float maxmotorTorque;
    private float CurrentCarTorque;
    private float maxSteerAngle;
    private float maxSpeedSteerAngle;
    float AccPer = 0.0f;//车辆加速度
    float CarAdjustParam = 0.0f;
    enum Wheel_Type
    {
        car_FL = 0,
        car_FR = 1
    }
    // Use this for initialization
    Transform[] Wheel = new Transform[WHEEL_COUNT];

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        //设置小车重心
        rigidbody.centerOfMass = new Vector3(0, 0, 0);
    }
    void Start()
    {
        carstatus = CarStatusType.LineRun;
        isCanTurn = false;
        DriftCar = false;
        CanDrift = false;
        DriftIndex = 0;
        IsAccelerate = false;
        IsAccelerateOver = false;
        Forward = Vector3.zero;
        Wheel[(int)Wheel_Type.car_FL] = this.transform.FindChild("CarWheel/car_FL");
        Wheel[(int)Wheel_Type.car_FR] = this.transform.FindChild("CarWheel/car_FR");
        r = GetComponent<References>();
        if (r != null)
        {
            MainCamera = r.Object[0].transform;
        }
        carMaxSpeed = DataManager.Instance.PlayerCarMaxSpeed;
        carAccSpeed = DataManager.Instance.PlayerCarAcceSpeed;
        maxSteerAngle = GameData.maxSteerAngle;
        maxSpeedSteerAngle = GameData.maxSpeedSteerAngle;
        maxmotorTorque = DataManager.Instance.PlayerCarMaxMotorTorque;
    }

    // Update is called once per frame
    void Update()
    {
        //CarAdjustParam = Input.GetAxis("Horizontal");
    }
    private void FixedUpdate()
    {
        currentspeed = (Mathf.PI * 2 * flWheelCollider.radius) * flWheelCollider.rpm * 60 / 1000;
        currentspeed = Mathf.Round(currentspeed);
        Debug.Log(currentspeed);
        if (!IsAccelerate)//正常行驶
        {
            if (currentspeed < carMaxSpeed)
            {
                AccPer = AccPer + 1 / carAccSpeed;
                AccPer = Mathf.Clamp(AccPer, 0, 1);
                flWheelCollider.motorTorque = maxmotorTorque*AccPer;
                frWheelCollider.motorTorque = maxmotorTorque * AccPer;
            }
            else
            {
                AccPer = AccPer - 1 / carAccSpeed;
                AccPer = Mathf.Clamp(AccPer, 0, 1);
                flWheelCollider.motorTorque = 0;
                frWheelCollider.motorTorque = 0;
            }
        }
        if(carstatus!=CarStatusType.LineRun)
        {
            float speedProcent = currentspeed / carMaxSpeed;
            speedProcent = Mathf.Clamp01(speedProcent);
            float speedControlledMaxSteerAngle;
            speedControlledMaxSteerAngle = maxSteerAngle - ((maxSteerAngle - maxSpeedSteerAngle) * speedProcent);
            flWheelCollider.steerAngle = speedControlledMaxSteerAngle * CarAdjustParam;
            frWheelCollider.steerAngle = speedControlledMaxSteerAngle * CarAdjustParam;
        }
        if(isCanTurn)
        {
            CarAdjustParam += Time.deltaTime * (carstatus==CarStatusType.LeftTurn ? -1 : 1);
        }
        else
        {
            CarAdjustParam -= Time.deltaTime * (carstatus == CarStatusType.LeftTurn ? -1 : 1);
        }
        CarAdjustParam = Mathf.Clamp(CarAdjustParam, -1, 1);
    }

    public void CarTurnAdjust(bool flag)//长按转向
    {
        isCanTurn = true;
        carstatus = (CarStatusType)(flag ? 1 : 2);
    }

    public void CarTurnOverAdjust(bool flag)
    {
        isCanTurn = false;
        carstatus = CarStatusType.LineRun;
        //todo:将轮胎调整为正常
        for (int i = 0; i < WHEEL_COUNT; i++)
        {
            Wheel[i].DORotate(this.transform.eulerAngles, 0.1f);
        }
    }

    public void DriftCarDown()
    {
        //TODO:进行速度判断进行漂移
        DriftCar = true;
        DriftBaseTime = Time.time;
        CanDrift = true;
        SpeedTemp = currentspeed;
    }
    public void DriftCarUp()
    {
        DriftCar = false;
    }
    public void OnAccelerateCar(GameObject go)
    {
        UIListener.SetBtnStatus(go,false);
        IsAccelerate = true;
        SpeedTemp = currentspeed;//保存发射前速度
        currentspeed = DataManager.Instance.PlayerCarShootSpeed;
    }
}
