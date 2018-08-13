using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraControler : MonoBehaviour {

    
    // Use this for initialization
    References r;
    GameObject CarObject;
    GameObject TargetObj;
    int CameraChaseIndex;
    void Start () {
        r = this.GetComponent<References>();
        if(r!=null)
        {
            CarObject = r.Object[0];
            TargetObj = r.Object[1];
        }
        CameraChaseIndex = 0;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        float CameraRealHeight = TargetObj.transform.position.y+GameData.CameraHight;
        float AngleCamera = Mathf.LerpAngle(transform.eulerAngles.y, TargetObj.transform.eulerAngles.y, GameData.SmoothTime*Time.deltaTime);
        Quaternion quaternion = Quaternion.Euler(0, AngleCamera, 0);

        transform.position = TargetObj.transform.position;
        transform.position = new Vector3(transform.position.x, CameraRealHeight,transform.position.z);
        if(CarControler.IsAccelerate)//汽车加速保持远距离
        {
            transform.position -= quaternion * Vector3.forward * GameData.CameraToCarDistanceAcce;
        }
        else
        {
            if(CarControler.IsAccelerateOver)//摄像机逼近
            {
                if(CameraChaseIndex<GameData.CameraChaseTimes)
                {
                    CameraChaseIndex++;
                    float speed = GameData.CameraToCarDistanceAcce - (CameraChaseIndex / GameData.CameraChaseTimes) * (GameData.CameraToCarDistanceAcce - GameData.CameraToCarDistanceNormal);//线性下降 
                    transform.position -= quaternion * Vector3.forward * speed;
                }
                else//摄像机跟上原来的速度
                {
                    CameraChaseIndex = 0;
                    CarControler.IsAccelerateOver = false;
                    transform.position -= quaternion * Vector3.forward * GameData.CameraToCarDistanceNormal;//bug 不加会出现摄像机跑过汽车再向后退的现象
                }
            }
            else
            {
                transform.position -= quaternion * Vector3.forward * GameData.CameraToCarDistanceNormal;
            }         
        }

        transform.LookAt(TargetObj.transform.position);//使摄像机一直观看目标

    }

}
