using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonClickListener : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerUpHandler
{
    // Use this for initialization
    public delegate void VoidDelegte(GameObject go);

    public VoidDelegte onClick;
    public VoidDelegte OnMouseDoubleClick;
    public VoidDelegte OnMouseEnter;
    public VoidDelegte OnMouseExit;
    public static ButtonClickListener AddEventListener(GameObject go)
    {
        ButtonClickListener listener = go.AddComponent<ButtonClickListener>();
        return listener;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(OnMouseEnter!=null)
        {
            OnMouseEnter(gameObject);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(OnMouseExit!=null)
        {
            OnMouseExit(gameObject);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnMouseDoubleClick != null)
        {
            if (eventData.clickCount == 2)
                OnMouseDoubleClick(gameObject);
        }
        if (onClick != null)
        {
            onClick(gameObject);           
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.1f).SetEase(Ease.Linear);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
    }
}
