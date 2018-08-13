using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour {

    public static string ResUrl
    {
        get
        {
            return "http://192.168.96.154/res1/";
        }
    }

    public static string PlatformPath
    {
        get
        {
#if UNITY_STANDALONE_WIN
            return "win";
#elif UNITY_ANDROID
            return "android";
#elif UNITY_IOS
            return "ios";
#endif
        }
    }

    // Use this for initialization
    private GameObject Root;
	void Start () {
        DontDestroyOnLoad(this.gameObject);
        Root = GameObject.FindWithTag("Root");
        if(Root!=null)
        {
            Root.AddComponent<GameEnter>();
        }
    }
	
	// Update is called once per frame
	void Update () {

    }
}
