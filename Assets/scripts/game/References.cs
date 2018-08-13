using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class References : MonoBehaviour {

    // Use this for initialization
    public GameObject[] Object;
	
    public T GetObjectComponentByIndex<T>(int index)where T:Component
    {
        return Object[index].GetComponent<T>();
    }

}
