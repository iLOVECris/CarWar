using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gprotocol;

public class ChangePlayerNamePanel : BasePanel {

    References reference;
    private GameObject CloseBtn;
    private GameObject CloseBg;
    private GameObject AddPhotoObj;
    private GameObject FaceObj;
    private GameObject AddPhotoBtn;
    private GameObject FaceIconRoot;
    private GameObject FirstTitle;
    int LastSelectFaceIcon;
    private Dictionary<int, FaceItem> FaceIconDic = new Dictionary<int, FaceItem>();
    private static ChangePlayerNamePanel _instance;
    public static ChangePlayerNamePanel Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UIManager.ChangePanel<ChangePlayerNamePanel>("ChangePlayerNamePanel", UIManager.BottomPanelRoot);
            }
            return _instance as ChangePlayerNamePanel;
        }
    }
    private void Awake()
    {
        panelname = "ChangePlayerNamePanel";
        LastSelectFaceIcon = PlayerManager.info.face;
        PlayerManager.LastPlayerFaceIcon = LastSelectFaceIcon;
        reference = GetComponent<References>();
        if(reference!=null)
        {
            CloseBg = reference.Object[0];
            if(CloseBg!=null)
            {
                CloseBg.AddComponent<ButtonClickListener>().onClick = ClosePanel;
            }
            CloseBtn = reference.Object[1];
            if(CloseBtn!=null)
            {
                CloseBtn.AddComponent<ButtonClickListener>().onClick = ClosePanel;
            }
            FaceObj = reference.Object[2];
            AddPhotoObj = reference.Object[3];
            AddPhotoBtn = reference.Object[4];
            FaceIconRoot = reference.Object[5];
            FirstTitle = reference.Object[6];
        }

    }
    private void Start()
    {
        InitFaceIcon();//生成item
        //TODO::更新添加图片root位置
    }
    public static void Show()
    {
        Instance.Open();
    }

    public override void Close()
    {
        base.Close();
        if(UIManager.DestroyPanel.Contains(panelname))
        {
            _instance = null;
        }
            
    }

    public override void Open()
    {
        base.Open();
    }

    private void ClosePanel(GameObject go)
    {
        Close();
        UserInfoPanel.Instance.UpdatePanel();
    }

    void InitFaceIcon()
    {
        Dictionary<int, FaceIconConfig> dic = FaceIconConfig.FaceIconDic;
        for (int index = 0;index< dic.Count;index++)
        {
            FaceIconConfig item = dic[index];
            CreateFaceItem(item);
        }      
        float offsetY = GameData.FaceIconWidth * Mathf.CeilToInt(((dic.Count) / GameData.FaceIconOneLineCount))+GameData.FaceTitleWidth;//根据头像的个数计算title与title的offset
        Vector3 LastAddPhotoObjPos = AddPhotoObj.transform.localPosition;
        AddPhotoObj.transform.localPosition = new Vector3(LastAddPhotoObjPos.x, FirstTitle.transform.localPosition.y-offsetY, LastAddPhotoObjPos.z);
        if(FaceIconDic!=null&& FaceIconDic.Count>0)
        {
            FaceIconDic[LastSelectFaceIcon].SetSelectFlag(true);
        }
    }
    void CreateFaceItem(FaceIconConfig item)
    {
        GameObject go = Instantiate(FaceObj);
        if(go!=null)
        {
            go.transform.parent = FaceIconRoot.transform;
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            go.SetActive(true);
            FaceItem face = go.AddComponent<FaceItem>();
            go.AddComponent<ButtonClickListener>().onClick = SelectFaceIcon;
            face.SetInfo(item);
            FaceIconDic.Add(item.id, face);
        }

    }

    void SelectFaceIcon(GameObject go)
    {
        FaceItem item = go.GetComponent<FaceItem>();
        if(item!=null)
        {
            if (LastSelectFaceIcon == item.id)//选择相同的
            {
                return;
            }
            item.SetSelectFlag(true);
            FaceIconDic[LastSelectFaceIcon].SetSelectFlag(false);
            LastSelectFaceIcon = item.id;
            ModifyPlayerIcon msg = new ModifyPlayerIcon();
            msg.playericon = item.id;
            TcpNet.Instance.send_proto_msg_to_client((int)Stype.Auth, (int)Cmd.eModifyPlayerIcon, msg);
            PlayerManager.LastPlayerFaceIcon = item.id;
        }
    }
}
