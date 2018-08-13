using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class FaceIconConfig : MonoBehaviour {

    public string resname;
    public int id;
    public static readonly string urlkey = "FaceIconConfig";
    public static Dictionary<int, FaceIconConfig> FaceIconDic = new Dictionary<int, FaceIconConfig>();
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
                    FaceIconConfig config = new FaceIconConfig();
                    config.id = int.Parse(e.GetAttribute("id"));
                    config.resname = e.GetAttribute("resname");
                    FaceIconDic.Add(config.id, config);
                }
            }
        }
    }

    public static string GetResNameById(int id)
    {
        return FaceIconDic[id].resname;
    }
}
