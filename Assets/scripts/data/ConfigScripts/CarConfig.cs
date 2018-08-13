using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class CarConfig : MonoBehaviour {

    private string carname;
    private string resname;
    private int id;
    public static readonly string urlkey = "CarConfig";
    public static Dictionary<int, CarConfig> AllCarDic = new Dictionary<int, CarConfig>();
    public static void Parse(TextAsset Text)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(Text.text);
        XmlNode Node = doc.SelectSingleNode("Config");
        if (Node != null)
        {
            XmlNodeList NodeList = Node.ChildNodes;
            if (NodeList != null && NodeList.Count > 0)
            {
                foreach (XmlElement e in NodeList)
                {
                    if (e == null || !e.GetType().Equals(typeof(XmlElement)))
                    {
                        continue;
                    }
                    CarConfig config = new CarConfig();
                    config.carname = e.GetAttribute("carname");
                    config.id = int.Parse(e.GetAttribute("id"));
                    config.resname = e.GetAttribute("resname");
                    AllCarDic.Add(config.id, config);
                }
            }
        }
    }

    public static float GetCarMaxSpeed(int index)
    { 
        float s = 65.25f;
        return s;
    }

    public static float GetCarAccelerateSpeed(int index)
    {
        float s = 3.6f;
        return s;
    }

    public static float GetCarDecelerationSpeed(int index)
    {
        float s = 2.6f;
        return s;
    }

    public static float GetCarMaxMotorTorque(int index)
    {
        float s = 92.25f;
        return s;
    }

    public static float GetCarShootSpeed(int index)
    {
        float s = 85.32f;
        return s;
    }
}
