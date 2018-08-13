using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainLobbyPanel : BasePanel {

    // Use this for initialization
    private GameObject LeftBtn;
    private GameObject RightBtn;
    private GameObject DragArea;
    float clickpos = 0.0f;
    string LastCarKey;
    string CarKey;
    const float CarBaseRotate = 182.7f;
    private static MainLobbyPanel _instance = null;
    public static MainLobbyPanel Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UIManager.ChangePanel<MainLobbyPanel>("MainLobbyPanel", UIManager.BottomPanelRoot);
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }
    void Awake()
    {
        panelname = "MainLobbyPanel";
        LeftBtn = this.transform.FindChild("Drag/LeftBtn").gameObject;
        if (LeftBtn!=null)
        {
            ButtonClickListener listener = ButtonClickListener.AddEventListener(LeftBtn);
            listener.onClick = Down;
        }
        RightBtn = this.transform.FindChild("Drag/RightBtn").gameObject;
        if(RightBtn!=null)
        {
            ButtonClickListener listener = ButtonClickListener.AddEventListener(RightBtn);
            listener.onClick = Down;
        }
        DragArea = this.transform.FindChild("Drag/DragArea").gameObject;
        CarTypeControler.CarRoot.position =new Vector3(DragArea.transform.position.x, DragArea.transform.position.y- 1.0f,0.0f);
        if (DragArea!=null)
        {
            EventTriggerListener listener = EventTriggerListener.AddEventListener(DragArea);
            listener.onDown = DragCar;
            listener.onDrag = CarOnDrag;
            listener.onUp = DragUp;
        }
    }
    public static void Show()
    {
        Instance.Open();
    }
    public override void Open()
    {
        base.Open();
    }
    public override void Close()
    {
        base.Close();
        if(UIManager.DestroyPanel.Contains(panelname))
        {
            Instance = null;
        }
    }
    private void CarOnDrag(GameObject go, float posx)
    {
        if (CarTypeControler.RotateCar != null)
        {
            if(clickpos==0)
            {
                clickpos = posx;
            }
            CarTypeControler.RotateCar.DORotate(CarTypeControler.RotateCar.eulerAngles - new Vector3(0.0f, (posx - clickpos) *GameData.RotateRadio, 0.0f), 0.1f);
            clickpos = posx;
        }
    }
    private void DragCar(GameObject go,float posx)
    {
        clickpos = posx;
    }
    private void Down(GameObject go)
    {
        if(go.name.Equals("LeftBtn"))
        {
            ChangeShowCar(-1);
        }
        else if(go.name.Equals("RightBtn"))
        {
            ChangeShowCar(1);
        }
    }
    public void DragUp(GameObject go, float pos)
    {
        CarTypeControler.RotateCar.DORotate(Vector3.zero+new Vector3(0.0f, CarBaseRotate, 0.0f)+new Vector3(1.98f, 12.94f, 0.0f), 0.1f);
    }
    public void ChangeShowCar(int index)
    {
        int TypeIndex = CarTypeControler.CarIndex;
        if ((TypeIndex <= 0 && index == -1) || (TypeIndex >= CarTypeControler.RootCarList.Count-1 && index == 1))
        {
            return;
        }
        CarTypeControler.CarIndex += index;
        CarKey = GameData.CarBaseName + CarTypeControler.CarIndex;
        CarTypeControler.Instance.ChangeCarToRoot(CarKey, CarTypeControler.CarRoot);
        LastCarKey = GameData.CarBaseName + TypeIndex;
        CarTypeControler.Instance.ChangeCarToRoot(LastCarKey, CarTypeControler.OtherRoot);
    }

}
