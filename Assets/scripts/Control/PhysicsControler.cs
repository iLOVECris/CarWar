using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PhysicsControler : MonoBehaviour {

    public WheelCollider flWheelCollider;
    public WheelCollider frWheelCollider;
    public WheelCollider rlWheelCollider;
    public WheelCollider rrWheelCollider;
    Rigidbody rigidbody;
    bool CanDrift;
    bool DriftOver;
    bool IsCanAdjust;
    bool AdjustDir;
    float maxSpeed;
    public float maxSteerAngle;
    public float maxSpeedSteerAngle;
    public float currentSpeed;
    float BeginTime;
    int DriftIndex;
    private const int WHEEL_COUNT = 2;
    Transform[] Wheel = new Transform[WHEEL_COUNT];
    enum Wheel_Type
    {
        car_FL = 0,
        car_FR = 1
    }
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        //设置小车重心
        rigidbody.centerOfMass = new Vector3(0, 0, 0);
        currentSpeed = 0.0f;
        maxSpeed = DataManager.Instance.PlayerCarMaxSpeed;
        maxSteerAngle = GameData.maxSteerAngle;
        maxSpeedSteerAngle = GameData.maxSpeedSteerAngle;
        BeginTime = 0;
        CanDrift = false;
        DriftOver = false;
        IsCanAdjust = false;
        AdjustDir = false;
        Wheel[(int)Wheel_Type.car_FL] = this.transform.FindChild("car_FL");
        Wheel[(int)Wheel_Type.car_FR] = this.transform.FindChild("car_FR");
    }

    void FixedUpdate()
    {

        
        //速度
        currentSpeed = (Mathf.PI * 2 * flWheelCollider.radius) * flWheelCollider.rpm * 60 / 1000;
        currentSpeed = Mathf.Round(currentSpeed);

        //速度小于最大速度
        if ((currentSpeed < maxSpeed))
        {
            //*Input.GetAxis("Vertical")
            currentSpeed = currentSpeed + Time.deltaTime * DataManager.Instance.PlayerCarAcceSpeed;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, DataManager.Instance.PlayerCarMaxSpeed);
            flWheelCollider.motorTorque = currentSpeed;
            frWheelCollider.motorTorque = currentSpeed;
        }
        else
        {
            flWheelCollider.motorTorque = 0;
            frWheelCollider.motorTorque = 0;
        }
        if (CanDrift)
        {
            float speedProcent = currentSpeed / maxSpeed;
            speedProcent = Mathf.Clamp(speedProcent, 0, 1);
            float speedControlledMaxSteerAngle;
            speedControlledMaxSteerAngle = maxSteerAngle - ((maxSteerAngle - maxSpeedSteerAngle) * speedProcent);
            flWheelCollider.steerAngle = speedControlledMaxSteerAngle;
            frWheelCollider.steerAngle = speedControlledMaxSteerAngle;
        }
        else
        {
            if(IsCanAdjust)
            {
                flWheelCollider.motorTorque = 0;
                frWheelCollider.motorTorque = 0;
                Vector3 adjustparam = AdjustDir ? new Vector3(0.0f, -GameData.CarAdjustParam, 0.0f) : new Vector3(0.0f, GameData.CarAdjustParam, 0.0f);
                Vector3 WheelAdjustparam = AdjustDir ? new Vector3(0.0f, -GameData.WheelAdjustParam, 0.0f) : new Vector3(0.0f, GameData.WheelAdjustParam, 0.0f);
                for (int i = 0; i < WHEEL_COUNT; i++)
                {
                    Wheel[i].DORotate(this.transform.eulerAngles + WheelAdjustparam, 0.1f);
                }

                this.transform.DORotate(this.transform.eulerAngles + adjustparam, 0.1f);
            }

        }
        if(DriftOver)
        {
            if (DriftIndex < GameData.DriftTimes)
            {
                DriftIndex++;
                flWheelCollider.steerAngle = GameData.maxSteerAngle * Input.GetAxis("Horizontal");
                frWheelCollider.steerAngle = GameData.maxSteerAngle * Input.GetAxis("Horizontal");
            }
            else
            {
                Debug.Log("over");
                flWheelCollider.steerAngle = 0;
                frWheelCollider.steerAngle = 0;
                DriftIndex = 0;
                DriftOver = false;
            }
        }
        
    }
    public void CarTurnAdjust(bool flag)//长按转向
    {
        IsCanAdjust = true;
        AdjustDir = flag;
    }

    public void CarTurnOverAdjust(bool flag)
    {
        IsCanAdjust = false;
    }

    public void DriftCarDown()
    {
        CanDrift = true;
    }
    public void DriftCarUp()
    {
        CanDrift = false;
        DriftOver = true;
    }
}
