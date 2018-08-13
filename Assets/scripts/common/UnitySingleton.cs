using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitySingleton<T>:MonoBehaviour where T:Component {

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private static T _instance;
    private static object mutex = new object();
    public static T Instance
    {
        get
        {
            if(_instance==null)
            {
                lock(mutex)
                {
                    _instance = FindObjectOfType<T>();
                    if(_instance==null)
                    {
                        GameObject go = new GameObject(typeof(T).Name);
                        _instance = go.AddComponent<T>();
                    }

                }
            }
            return _instance;
        }
    }

}
