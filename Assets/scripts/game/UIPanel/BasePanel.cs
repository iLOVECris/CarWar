using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour {
    public string panelname;  
    public virtual void Close()//todo:界面移动到缓冲池中
    {
        if(!UIManager.PanelPool.ContainsKey(this.panelname))
        {
            if (UIManager.DestroyPanel.Contains(panelname))//需要删除
            {
                Destroy(this.gameObject);
            }
            else
            {
                this.gameObject.SetActive(false);
                UIManager.PanelPool.Add(this.panelname, this);
            }
        }
    }
    public virtual void Open()//todo：从缓冲池取出,
    {
        if (UIManager.PanelPool.ContainsKey(this.panelname))
        {
            this.gameObject.SetActive(true);
            UIManager.PanelPool.Remove(this.panelname);
        }
    }
    public void init()
    {
        this.transform.localScale = Vector3.one;
        this.transform.localPosition = Vector3.zero;
    }
    public void Destroy()//直接销毁，不放到缓冲池
    {
        DestroyObject(this);
    }
}
