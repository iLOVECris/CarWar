using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyChatItem : BasePanel {

    // Use this for initialization
    Text content;
    RectTransform ContentRect;
    RectTransform ChatItemRect;
    Texture faceicon;
    ContentSizeFitter cf;
    bool changeline = false;

    void Awake () {
        content = transform.Find("content").GetComponent<Text>();
        if (content != null)
        {
            ContentRect = content.GetComponent<RectTransform>();
            cf = content.GetComponent<ContentSizeFitter>();
        }
        faceicon = transform.Find("faceicon/avata").GetComponent<RawImage>().texture;
        ChatItemRect = transform.Find("bg").GetComponent<RectTransform>();
    }

    public void setcontext(string msg)
    {
        content.text = msg;
    }
    public void setfaceicon()
    {

    }
    private void Update()
    {
        float width = ContentRect.sizeDelta.x;
        float height = ChatItemRect.sizeDelta.y;
        if (width>GameData.ChatLineMaxSize)//换行
        {
            int len = content.text.Length;
            content.text = content.text.Substring(0, (len / 2)+1) + "\n" + content.text.Substring((len / 2)+1, len - len / 2-1);
            content.alignment = TextAnchor.UpperLeft;
            changeline = true;
            cf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            Vector2 rect = ContentRect.sizeDelta = new Vector2(width/2, ChatItemRect.sizeDelta.y);          
            ChatItemRect.sizeDelta = new Vector2(rect.x + GameData.ChatItemBaseLength+10, rect.y);//+10为缓冲区大小
        }
        else
        {
            if(!changeline)
            {
                content.alignment = TextAnchor.MiddleLeft;
                cf.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
                ContentRect.sizeDelta = new Vector2(width + GameData.ChatItemBaseLength, GameData.ChatItemBaseLineHeight);
                ChatItemRect.sizeDelta = ContentRect.sizeDelta;
            }         
        }
       
    }
}
