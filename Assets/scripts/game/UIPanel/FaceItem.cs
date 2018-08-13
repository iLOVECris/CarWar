using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceItem : MonoBehaviour {

    // Use this for initialization
    private Image SelectFlagImage;
    private RawImage FaceIcon;
    public int id;
    Color NormalColor = new Color32(255, 255, 255, 33);
    Color SelectColor = new Color32(255, 255, 255, 255);
    void Awake()
    {
        SelectFlagImage = GetComponent<Image>();
        FaceIcon = transform.FindChild("icon").GetComponent<RawImage>();
    }
	public void SetInfo(FaceIconConfig item)
    {
        id = item.id;
        if (FaceIcon!=null)
        {
            FaceIcon.texture = ResManager.GetResource<Texture>(item.resname);
        }
    }
    public void SetSelectFlag(bool select)
    {
        SelectFlagImage.color = select?SelectColor:NormalColor;
    }
}
