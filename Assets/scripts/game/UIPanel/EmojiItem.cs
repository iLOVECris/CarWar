using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Debug_NameSpace;

public class EmojiItem : MonoBehaviour {

    // Use this for initialization
    private int emoji_id;
    public int Emoji_Id
    {
        get
        {
            return emoji_id;
        }
        set
        {
            emoji_id = value;
        }
    }
    Image emoji;
	void Awake ()
    {
        emoji = GetComponent<Image>();
    }
	public void SetEmoji(string resname)
    {
        Sprite s = ResManager.GetResource<Sprite>(resname);
        if(s==null)
        {
            Log_Debug.LogError("is null " + resname);
        }
        else
        {
            emoji.sprite = s;
        }
    }
}
