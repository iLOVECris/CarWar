using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendItem : BasePanel
{
    Image bg;
    Text LastContent;
    Text PlayerName;
    Texture face;
    void Awake()
    {
        bg = GetComponent<Image>();
        PlayerName = transform.Find("name").GetComponent<Text>();
        LastContent = transform.Find("lastcontent").GetComponent<Text>();
        face = transform.Find("mask/face").GetComponent<RawImage>().texture;
    }

	// Use this for initialization
	void Start () {
		
	}
	public void SetPlayerName(string name)
    {
        PlayerName.text = name;
    }
    public void SetLastContent(string content)
    {
        LastContent.text = content;
    }
}
