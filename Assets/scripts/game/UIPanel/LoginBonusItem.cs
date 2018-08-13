using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using gprotocol;
public enum LoginSignType
{
    WaitSign = 0,//待签到
    CanSign = 1,//可签到
    AgainSign = 2,//可补签
    AlreadySign = 3,//已经签到
}

public class LoginBonusItem : MonoBehaviour {

    private References ref_LoginBonusItem;
    private Image Image_frameBg;
    private Image Image_bg;
    private Text Sign_text;//签到状态
    private Text Day_text;
    private Text Bonus_text;
    private Image Bonus_Image;
    private Image Image_mask;
    public LoginSignType signtype;
    public int DayIndex;
    void Awake()
    {
        ref_LoginBonusItem = GetComponent<References>();
        Image_frameBg = GetComponent<Image>();
        if (ref_LoginBonusItem!=null)
        {
            Image_bg = ref_LoginBonusItem.Object[0].GetComponent<Image>();
            Sign_text = ref_LoginBonusItem.Object[1].GetComponent<Text>();
            Bonus_text = ref_LoginBonusItem.Object[2].GetComponent<Text>();
            Day_text = ref_LoginBonusItem.Object[3].GetComponent<Text>();
            Bonus_Image = ref_LoginBonusItem.Object[4].GetComponent<Image>();
            Image_mask = ref_LoginBonusItem.Object[5].GetComponent<Image>();
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	public void SetInfo(LoginBonusData data)
    {
        EquipConfig eqt = EquipConfig.GetEquipByID(data.id);
        signtype = (LoginSignType)data.status;
        switch (data.status)
        {
            case (int)LoginSignType.WaitSign:
                Sign_text.text = "待签到";
                break;
            case (int)LoginSignType.CanSign:
                Sign_text.text = "可签到";
                break;
            case (int)LoginSignType.AgainSign:
                Sign_text.text = "可补签";
                break;
            case (int)LoginSignType.AlreadySign:
                Sign_text.text = "已领取";
                break;
        }
        DayIndex = data.day;
        Bonus_text.text = data.num + eqt.equipname;
        Day_text.text = "第" + data.day + "天";
        Bonus_Image.sprite = ResManager.GetResource<Sprite>(eqt.resname);
        Image_mask.gameObject.SetActive(data.status == 3);
    }
}