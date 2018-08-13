using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIListener:MonoBehaviour {

    // Use this for initialization
    References r;
    GameObject LeftBtn;
    GameObject RightBtn;
    GameObject AccelerateBtn;
    GameObject DriftBtn;
    CarControler carControler;
    //PhysicsControler phyControler;

    void Start() {
        r = GetComponent<References>();
        if(r!=null)
        {
            LeftBtn = r.Object[0];
            RightBtn = r.Object[1];
            DriftBtn = r.Object[3];
            AccelerateBtn = r.Object[4];
            if (LeftBtn!=null)
            {
                EventTriggerListener LeftTrigger = LeftBtn.AddComponent<EventTriggerListener>();
                LeftTrigger.onDown = AdjustOnPointerDownCallBack;
                LeftTrigger.onUp = AdjustOnPointerUpCallBack;
            }
            if(RightBtn!=null)
            {
                EventTriggerListener RightTrigger = RightBtn.AddComponent<EventTriggerListener>();
                RightTrigger.onDown = AdjustOnPointerDownCallBack;
                RightTrigger.onUp = AdjustOnPointerUpCallBack;
            }
            if (DriftBtn != null)
            {
                EventTriggerListener DriftTrigger = DriftBtn.AddComponent<EventTriggerListener>();
                DriftTrigger.onDown = DriftCarBtnDown;
                DriftTrigger.onUp = DriftCarBtnUp;
            }
            if(AccelerateBtn!=null)
            {
                EventTriggerListener AccerateTrigger = AccelerateBtn.AddComponent<EventTriggerListener>();
                AccerateTrigger.onDown = AccelerateBtnDown;
            }
            carControler = r.Object[2].GetComponent<CarControler>();
            //phyControler = r.Object[2].GetComponent<PhysicsControler>();
        }
	}

    public static void SetBtnStatus(GameObject go,bool status)
    {
        Button btn = go.GetComponent<Button>();
        if(btn!=null)
        {
            btn.interactable = status ? true : false;
        }
        EventTriggerListener Trigger = go.GetComponent<EventTriggerListener>();
        if (Trigger != null)
        {
            Trigger.enabled = status ? true : false;
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
    void AdjustOnPointerDownCallBack(GameObject go,float pos)
    {
        if (carControler != null)
        {
            if (go.name.Equals("LeftBtn"))
            {
                carControler.CarTurnAdjust(true);
            }
            else if (go.name.Equals("RightBtn"))
            {
                carControler.CarTurnAdjust(false);
            }
        }

    }
    void AdjustOnPointerUpCallBack(GameObject go,float pos)
    {
        if (carControler != null)
        {
            if (go.name.Equals("LeftBtn"))
            {
                carControler.CarTurnOverAdjust(true);
            }
            else if (go.name.Equals("RightBtn"))
            {
                carControler.CarTurnOverAdjust(false);
            }
        }
    }
    void DriftCarBtnDown(GameObject go,float pos)
    {
        if(carControler != null)
        {
            carControler.DriftCarDown();
        }
    }

    void DriftCarBtnUp(GameObject go,float pos)
    {
        if (carControler != null)
        {
            carControler.DriftCarUp();
        }
    }

    void AccelerateBtnDown(GameObject go,float pos)
    {
        if (carControler != null)
        {
            carControler.OnAccelerateCar(go);
        }
    }
    private void FixedUpdate()
    {      
        if(Input.GetKeyDown(KeyCode.Space))
        {
            carControler.OnAccelerateCar(AccelerateBtn);
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            carControler.DriftCarDown();
        }
        if(Input.GetKeyUp(KeyCode.L))
        {
            carControler.DriftCarUp();
        }
    }
}
