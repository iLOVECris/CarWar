using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class ErrCodeConfig : MonoBehaviour {

    private string tips;
    private int id;
    public static readonly string urlkey = "ErrCodeConfig";
    public static Dictionary<int, ErrCodeConfig> AllErrCodeDic = new Dictionary<int, ErrCodeConfig>();
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
                    ErrCodeConfig config = new ErrCodeConfig();
                    config.tips = e.GetAttribute("tips");
                    config.id = int.Parse(e.GetAttribute("id"));
                    AllErrCodeDic.Add(config.id, config);
                }
            }
        }
    }

    public static string GetTipsById(int id)
    {
        if(AllErrCodeDic.ContainsKey(id))
        {
            return AllErrCodeDic[id].tips;
        }
        return null;
    }
}
