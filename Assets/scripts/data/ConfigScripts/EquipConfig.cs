using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class EquipConfig : MonoBehaviour {

    public string resname;
    public string equipname;
    public int id;
    public int equiptype;
    public static readonly string urlkey = "EquipConfig";
    public static Dictionary<int, EquipConfig> AllEqtDic = new Dictionary<int, EquipConfig>();
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
                    EquipConfig config = new EquipConfig();
                    config.resname = e.GetAttribute("resname");
                    config.id = int.Parse(e.GetAttribute("id"));
                    config.equipname = e.GetAttribute("equipname");
                    config.equiptype = int.Parse(e.GetAttribute("equiptype"));
                    AllEqtDic.Add(config.id, config);
                }
            }
        }
    }

    public static EquipConfig GetEquipByID(int ID)
    { 
        if(AllEqtDic.ContainsKey(ID))
        {
            return AllEqtDic[ID];
        }
        return new EquipConfig();
    }
    public static string GetEquipResNameByID(int ID)
    {
        if (AllEqtDic.ContainsKey(ID))
        {
            return AllEqtDic[ID].resname;
        }
        return "";
    }

    public bool IsMoney()
    {
        return equiptype == 1;
    }

}
