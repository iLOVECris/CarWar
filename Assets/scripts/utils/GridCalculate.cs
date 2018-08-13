using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridCalculate : MonoBehaviour {
    RectTransform r;
    float SpaceValueX;
    float SpaceValueY;
    float CellSizeY;
    float CellSizeX;
    float toppadding;
    float LineElems;
    // Use this for initialization
    void Start () {
        r = this.GetComponent<RectTransform>();
        GridLayoutGroup gridinfo = this.GetComponent<GridLayoutGroup>();
        if(gridinfo)
        {
            SpaceValueX = gridinfo.spacing.x;
            SpaceValueY = gridinfo.spacing.y;
            CellSizeY = gridinfo.cellSize.y;
            CellSizeX = gridinfo.cellSize.x;
            toppadding = gridinfo.padding.top;
        }
        if (gridinfo.constraint == GridLayoutGroup.Constraint.Flexible)
        {
            LineElems = (r.sizeDelta.x - gridinfo.padding.left - gridinfo.padding.right) / (CellSizeX + SpaceValueX);
        }
        else if (gridinfo.constraint == GridLayoutGroup.Constraint.FixedColumnCount)//限制列数
        {
            LineElems = gridinfo.constraintCount;
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (LineElems > 1)
        {
            r.sizeDelta = new Vector2(r.rect.width, (SpaceValueY + CellSizeY) * (Mathf.Ceil((float)this.transform.childCount / LineElems)) - SpaceValueY + toppadding);
        }
        else if(LineElems==1)
        {
            r.sizeDelta = new Vector2(r.rect.width, (SpaceValueY + CellSizeY) * ((this.transform.childCount / LineElems)) - SpaceValueY + toppadding);
        }
        else
        {
            return;
        }
    }
}
