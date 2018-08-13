using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using gprotocol;
using DG.Tweening;

public struct RewardStruct
{
    public int rewardid;
    public int rewardnum;
    public RewardStruct(GetLoginBonusRes ret)
    {
        this.rewardid = ret.id;
        this.rewardnum = ret.num;
    }
}
public class LoginBonusRewardPanel : BasePanel {

    public static LoginBonusRewardPanel Instance
    {
        get
        {
            if(_instance==null)
            {
                _instance = UIManager.ChangePanel<LoginBonusRewardPanel>("LoginBonusRewardPanel", UIManager.PopPanelRoot);
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }
    private static LoginBonusRewardPanel _instance;

    Image rewardImage;
    GameObject bg;
    Text rewardnum;
    Queue<RewardStruct> RewardQueue;
    private bool IsCanClose = false;
    void Awake()
    {
        panelname = "LoginBonusRewardPanel";
        References reference = GetComponent<References>();
        if(reference!=null)
        {
            rewardImage = reference.GetObjectComponentByIndex<Image>(0);
            rewardnum = reference.GetObjectComponentByIndex<Text>(1);
            bg = reference.Object[2];
            if(bg!=null)
            {
                bg.AddComponent<ButtonClickListener>().onClick = ClosePanel;
            }
        }
       
    }
	// Use this for initialization
	void Start () {
		
	}
    #region public
    public static void Show(List<RewardStruct>list)
    {
        Instance.RewardQueue = new Queue<RewardStruct>(list);
        Instance.Open();
    }
    public override void Open()
    {
        base.Open();
        StartCoroutine(ShowRewardAnima());
    }
    public override void Close()
    {
        base.Close();
    }
    #endregion


    #region private
    private void ClosePanel(GameObject obj)
    {
        if(IsCanClose)
            Close();
    }

    IEnumerator ShowRewardAnima()
    {
        if(RewardQueue.Count > 0)
        {
            yield return ShowReward(RewardQueue.Dequeue());
        }       
    }
    IEnumerator ShowReward(RewardStruct reward)
    {
        rewardImage.sprite = ResManager.GetResource<Sprite>(EquipConfig.GetEquipResNameByID(reward.rewardid));
        rewardnum.text = reward.rewardnum.ToString();
        rewardImage.transform.DOScale(Vector3.one, 0.378f).SetDelay(0.195f).SetEase(Ease.OutBack).OnComplete(delegate ()
        {
            if (RewardQueue.Count>0)
            {
                rewardImage.transform.DOScale(Vector3.zero,0.3f).SetDelay(2.0f).OnComplete(delegate { StartCoroutine(ShowRewardAnima());});
            }
            else
            {
                IsCanClose = true;
            }
        });
        yield return new WaitForEndOfFrame();
    }

    #endregion
}