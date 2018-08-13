using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopItem : BasePanel {

    RectTransform Bg_Rect;
    RectTransform Text_Rect;
    Text tips;
    // Use this for initialization
    private void Awake()
    {
        Bg_Rect = this.GetComponent<RectTransform>();
        tips = this.transform.FindChild("Tips").GetComponent<Text>();
        Text_Rect = tips.gameObject.GetComponent<RectTransform>();
    }
    void Start () {
        
    }
    public void SetTips(string s)
    {
        tips.text = s;
        this.transform.DOLocalMoveY(GameData.PopItemMove, 0.68f).SetEase(Ease.Linear).OnComplete(delegate ()
        {
            gameObject.SetActive(false);
            UIManager.PopItemPool.Enqueue(this);
        }
        );
    }
    // Update is called once per frame
    void Update()
    {
        Bg_Rect.sizeDelta = new Vector2(Text_Rect.rect.width + 20, Bg_Rect.rect.height);
    }
}
