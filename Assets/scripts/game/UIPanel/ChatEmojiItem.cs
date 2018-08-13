using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatEmojiItem : BasePanel {

    Image bg;
    Texture faceicon;
    const string basename = "emoji_";
    void Awake()
    {
        faceicon = transform.Find("faceicon/avata").GetComponent<RawImage>().texture;
        bg = transform.Find("bg").GetComponent<Image>();
    }

    public void SetIcon(int id)
    {
        string name = basename + id.ToString();
        bg.sprite = ResManager.GetResource<Sprite>(name);
    }
}
