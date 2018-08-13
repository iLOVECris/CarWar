using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CarTypeControler : MonoBehaviour
{
    // Use this for initialization
    public static GameObject UIRoot;
    public static Transform RotateCar;
    public static Transform OtherRoot;
    public static Transform CarRoot;
    private string carbasename;
    private Camera RootCamera;
    public static int CarIndex;  
    float coefficient;
    private float offset;
    string CarKey;
    public static CarTypeControler Instance;
    public static Dictionary<string,GameObject> RootCarList = new Dictionary<string,GameObject>();
    const int RootLayer = 9;

    void Awake ()
    {
        Instance = this;
        UIRoot = GameObject.Find("UIRoot");
        CarIndex = 0;
        coefficient = GameData.FadeEaseBase;
        carbasename = GameData.CarBaseName;
        OtherRoot = this.transform.FindChild("OtherCarRoot");
        CarRoot = this.transform.FindChild("CarRoot");
        RootCamera = this.transform.FindChild("3DRootCamera").GetComponent<Camera>();
        AddCarToRoot();
        CarKey = carbasename + CarIndex;
        ChangeCarToRoot(CarKey, CarRoot);
        RootCamera.gameObject.SetActive(false);
        RootCamera.cullingMask = 1<< RootLayer;
        RootCamera.clearFlags = CameraClearFlags.Depth;
        RootCamera.gameObject.SetActive(true);
    }

    void AddCarToRoot()//添加所有的car到other中
    {
        for (int i=0;i< CarConfig.AllCarDic.Count; i++)//
        {
            string basestr = carbasename + i;
            GameObject go = ResManager.GetResource<GameObject>(basestr);
            GameObject obj = Instantiate(go);
            obj.transform.parent = OtherRoot;
            obj.transform.localScale = new Vector3(coefficient, coefficient, coefficient);
            obj.transform.localRotation = Quaternion.Euler(Vector3.zero);
            obj.transform.localPosition = Vector3.zero;
            if(obj!=null)
            {
                RootCarList.Add(basestr,obj);
            }
        }
    }

    public void ChangeCarToRoot(string name,Transform root)//移除一个car到root
    {
        GameObject go;
        if(RootCarList.TryGetValue(name,out go))
        {
            if(go!=null)
            {
                if(root.name.Equals("CarRoot"))
                {
                    RotateCar = go.transform;
                }
                go.transform.parent = root;
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.Euler(new Vector3(1.98f,12.94f,0.0f));
                go.transform.localScale = new Vector3(coefficient, coefficient, coefficient);
                go.transform.DOScale(Vector3.one, GameData.CarFadeTime).SetEase(Ease.OutBack);
            }
        }

    }

}
