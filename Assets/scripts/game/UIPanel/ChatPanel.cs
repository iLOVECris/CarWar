using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using gprotocol;
using Debug_NameSpace;
using DG.Tweening;
using gprotocol;

public class ChatPanel : BasePanel
{
    // Use this for initialization
    private References r;
    GameObject panel;
    private GameObject ChatEnter;
    private GameObject ChatRoot;
    public static bool ShowChat;
    private GameObject FaceIcon;
    private InputField SendText;
    private GameObject SendBtn;

    private Text ChatTips;
    GridLayoutGroup ScrollGrid;
    ScrollRect Scroll;
    RectTransform ScrollRect;//滑动区域的Rect
    RectTransform GridRect;
    float CellHeight;//每组的大小
    GameObject EmojiBg;//表情背景图
    GameObject EmojiItemRoot;
    const string EmojiName = "emoji_";
    private static ChatPanel _instance;
    public static ChatPanel Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = UIManager.ChangePanel<ChatPanel>("ChatPanel", UIManager.TopPanelRoot);
            }
            return _instance as ChatPanel;
        }
    }
    private void Awake()
    {
        panelname = "ChatPanel";
        transform.localPosition = new Vector3(500, 0, 0);
        panel = transform.Find("Panel").gameObject;
        ChatEnter = this.transform.Find("ChatEnter").gameObject;
        if (ChatEnter != null)
        {
            ButtonClickListener listener = ButtonClickListener.AddEventListener(ChatEnter);
            listener.onClick = OnClick;
        }

        ShowChat = false;
        if (panel!=null)
        {
            r = panel.GetComponent<References>();
            if (r != null)
            {
                ChatTips = r.Object[0].GetComponent<Text>();
                FaceIcon = r.Object[3];
                if (FaceIcon != null)
                {
                    FaceIcon.AddComponent<ButtonClickListener>().onClick = FacePanelOpen;
                }
                SendBtn = r.Object[4];
                if (SendBtn != null)
                {
                    SendBtn.AddComponent<ButtonClickListener>().onClick = SendMsg;
                }
                SendText = r.Object[2].GetComponent<InputField>();
                ChatRoot = r.Object[1];
                if (ChatRoot != null)
                {
                    ScrollGrid = ChatRoot.GetComponent<GridLayoutGroup>();
                    CellHeight = ScrollGrid.spacing.y + ScrollGrid.cellSize.y;
                    GridRect = ChatRoot.GetComponent<RectTransform>();
                }
                Scroll = r.Object[5].GetComponent<ScrollRect>();
                ScrollRect = r.Object[5].GetComponent<RectTransform>();
                EmojiBg = r.Object[6];
                if (!EmojiBg.activeInHierarchy)
                {
                    EmojiBg.SetActive(true);
                }
                EmojiItemRoot = r.Object[7];
                if (EmojiBg != null)
                {
                    EmojiBg.AddComponent<ButtonClickListener>().onClick = CloseEmojiPanel;
                }
            }
        }
        TcpNet.Instance.RegisterServiceHandler((int)Stype.TalkRoom, TalkRoomServiceEventHandler);
        RegisterTalkRoomServiceCmdHandler();
    }
    void Start () {
        AddEmojiToRoot();
        EmojiBg.SetActive(false);
        send();
    }
    public static void Show()
    {
        Instance.Open();
    }
    public override void Open()
    {
        base.Open();
    }
    void send()
    {
        gprotocol.LoginReq req = new gprotocol.LoginReq();
        req.name = "blake";
        req.email = "blake@bycw.edu";
        req.age = 34;
        req.int_set = 8;

        TcpNet.Instance.send_proto_msg_to_client((int)Stype.TalkRoom, (int)Cmd.eLoginReq, req);
    }
    #region 
    void RegisterTalkRoomServiceCmdHandler()
    {
        messagedata data = new messagedata();
        data.RegisterCmdCB<Respose>((int)Cmd.eRespose, OnLoginRes);//添加消息回调
        data.RegisterCmdCB<RecvMsg>((int)Cmd.eRecvMsg, OnRecvMsg);
        data.RegisterCmdCB<RecvEmoji>((int)Cmd.eRecvEmoji, OnRecvEmoji);
        TcpNet.ServiceDic.Add((int)Stype.TalkRoom, data);
    }
    public static void OnLoginRes(Respose ret)
    {
        Debug.Log(ret.status);
    }
    public static void OnRecvEmoji(RecvEmoji msg)
    {
        UIManager.chatpanel.RecvEmojiMsg(msg.id);
    }
    public static void OnRecvMsg(RecvMsg msg)
    {
        if(UIManager.chatpanel==null)
        {
            Debug.Log("chatpanel is null");
            return;
        }
        UIManager.chatpanel.RecvMsg(msg.msg);
    }
    void TalkRoomServiceEventHandler(msg_cmd msg)//注册服务回调---待优化
    {
        if (TcpNet.Instance.Protocol_type == Protocol_Type.protocol_protobuf)
        {
            MsgCallBack call =TcpNet.ServiceDic[msg.stype].MsgCallBackDic[msg.ctype];
            object value = null;
            try
            {
                value = DecodeCmd.Deserialize(msg.body, call.msg);
            }
            catch (Exception e)
            {
                Log_Debug.LogError("序列化消息失败");
            }
            call.callback.Method.Invoke(null, new object[] { value });
        }
        else
        {
            string cmd = System.Text.Encoding.UTF8.GetString(msg.body);
        }

    }

    #endregion

    private void AddEmojiToRoot()
    {
        string resname = null;
        for (int index = 1; index < (GameData.EmojiCount + 1); index++)
        {
            resname = EmojiName + index.ToString();
            EmojiItem item = UIManager.AddItem<EmojiItem>("EmojiItem", EmojiItemRoot.transform);
            item.SetEmoji(resname);
            item.Emoji_Id = index;
            ButtonClickListener listener = item.gameObject.AddComponent<ButtonClickListener>();
            listener.onClick = SendEmoji;
        }
    }

    void FacePanelOpen(GameObject go)
    {
        if(!EmojiBg.activeInHierarchy)
        {
            EmojiBg.SetActive(true);
        }
    }

    void RefreshScrollRect()
    {
        float ShowArea = ScrollRect.sizeDelta.y / CellHeight;
        float OverArea = (ScrollGrid.transform.childCount - ShowArea) * CellHeight;
        GridRect.anchoredPosition = new Vector2(GridRect.anchoredPosition.x, OverArea);
    }

    public void RecvEmojiMsg(int emoji_id)
    {
        if (emoji_id <= 0)
        {
            return;
        }
        CreateEmojiMsg(emoji_id, false, ChatRoot.transform);
        if (ScrollGrid.transform.childCount > (ScrollRect.sizeDelta.y / CellHeight))
            RefreshScrollRect();
    }

    public void RecvMsg(string msg)
    {
        CreateOneMsg(msg, false, ChatRoot.transform);
        if (ScrollGrid.transform.childCount > (ScrollRect.sizeDelta.y / CellHeight))
            RefreshScrollRect();
    }
    void SendEmoji(GameObject go)
    {
        int id = go.GetComponent<EmojiItem>().Emoji_Id;
        if(id<=0)
        {
            return;
        }
        CreateEmojiMsg(id,true, ChatRoot.transform);
        if (ScrollGrid.transform.childCount > (ScrollRect.sizeDelta.y / CellHeight))
            RefreshScrollRect();
        EmojiBg.SetActive(false);
        SendEmoji _msg = new SendEmoji();
        _msg.id = id;
        TcpNet.Instance.send_proto_msg_to_client((int)Stype.TalkRoom, (int)Cmd.eSendEmoji, _msg);
    }
    void SendMsg(GameObject go)
    {
        string msg = SendText.text;
        if(string.IsNullOrEmpty(msg))
        {
            return;
        }
        CreateOneMsg(msg,true, ChatRoot.transform);
        if(ScrollGrid.transform.childCount> (ScrollRect.sizeDelta.y / CellHeight))
            RefreshScrollRect();
        SendMsg _msg = new SendMsg();
        _msg.msg = SendText.text;
        TcpNet.Instance.send_proto_msg_to_client((int)Stype.TalkRoom, (int)Cmd.eSendMsg, _msg);
        SendText.text = string.Empty;


    }
    void CreateEmojiMsg(int emoji_id,bool isme, Transform root)//添加的是emoji预制件
    {
        GameObject go = null;
        ChatEmojiItem item = null;
        if (isme)
        {
            item = UIManager.AddItem<ChatEmojiItem>("EmojiItemSelf", root);
        }
        else
        {
            item = UIManager.AddItem<ChatEmojiItem>("EmojiItemOther", root);     
        }
        item.SetIcon(emoji_id);
    }
    void CreateOneMsg(string msg,bool isme,Transform root)
    {
        GameObject go = null;
        MyChatItem item = null;
        if (isme)
        {
            item = UIManager.AddItem<MyChatItem>("MyChatItem",root);

        }
        else
        {
            item = UIManager.AddItem<MyChatItem>("OtherChatItem", root);
        }
        item.setcontext(msg);
    }
    void CloseEmojiPanel(GameObject go)
    {
        EmojiBg.SetActive(false);
    }

    void OnClick(GameObject go)
    {
        ShowChat = !ShowChat;
        ShowChatWindow(ShowChat);
    }

    void ShowChatWindow(bool hide)
    {
        if (hide)
        {
            transform.DOLocalMoveX(0, 0.2f).SetEase(Ease.InOutQuint);
        }
        else
        {
            transform.DOLocalMoveX(500, 0.2f).SetEase(Ease.InOutQuint);
        }
    }
}
