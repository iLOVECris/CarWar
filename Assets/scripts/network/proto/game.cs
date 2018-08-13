//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: game.proto
namespace gprotocol
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LoginReq")]
  public partial class LoginReq : global::ProtoBuf.IExtensible
  {
    public LoginReq() {}
    
    private string _name;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string name
    {
      get { return _name; }
      set { _name = value; }
    }
    private int _age;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"age", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int age
    {
      get { return _age; }
      set { _age = value; }
    }
    private string _email;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"email", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string email
    {
      get { return _email; }
      set { _email = value; }
    }
    private int _int_set;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"int_set", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int int_set
    {
      get { return _int_set; }
      set { _int_set = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"GuestLogin")]
  public partial class GuestLogin : global::ProtoBuf.IExtensible
  {
    public GuestLogin() {}
    
    private string _u_key;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"u_key", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string u_key
    {
      get { return _u_key; }
      set { _u_key = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"GuestLoginInfo")]
  public partial class GuestLoginInfo : global::ProtoBuf.IExtensible
  {
    public GuestLoginInfo() {}
    
    private int _uid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"uid", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int uid
    {
      get { return _uid; }
      set { _uid = value; }
    }
    private string _name;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string name
    {
      get { return _name; }
      set { _name = value; }
    }
    private int _sex;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"sex", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int sex
    {
      get { return _sex; }
      set { _sex = value; }
    }
    private int _face;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"face", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int face
    {
      get { return _face; }
      set { _face = value; }
    }
    private int _is_guest;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"is_guest", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int is_guest
    {
      get { return _is_guest; }
      set { _is_guest = value; }
    }
    private int _status;
    [global::ProtoBuf.ProtoMember(6, IsRequired = true, Name=@"status", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int status
    {
      get { return _status; }
      set { _status = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"GuestLoginRes")]
  public partial class GuestLoginRes : global::ProtoBuf.IExtensible
  {
    public GuestLoginRes() {}
    
    private int _errcode;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"errcode", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int errcode
    {
      get { return _errcode; }
      set { _errcode = value; }
    }
    private GuestLoginInfo _info = null;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public GuestLoginInfo info
    {
      get { return _info; }
      set { _info = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Respose")]
  public partial class Respose : global::ProtoBuf.IExtensible
  {
    public Respose() {}
    
    private int _status;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"status", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int status
    {
      get { return _status; }
      set { _status = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"SendMsg")]
  public partial class SendMsg : global::ProtoBuf.IExtensible
  {
    public SendMsg() {}
    
    private string _msg;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"msg", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string msg
    {
      get { return _msg; }
      set { _msg = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"SendEmoji")]
  public partial class SendEmoji : global::ProtoBuf.IExtensible
  {
    public SendEmoji() {}
    
    private int _id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int id
    {
      get { return _id; }
      set { _id = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"RecvMsg")]
  public partial class RecvMsg : global::ProtoBuf.IExtensible
  {
    public RecvMsg() {}
    
    private string _msg;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"msg", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string msg
    {
      get { return _msg; }
      set { _msg = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ModifyName")]
  public partial class ModifyName : global::ProtoBuf.IExtensible
  {
    public ModifyName() {}
    
    private string _name;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string name
    {
      get { return _name; }
      set { _name = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ModifyPlayerIcon")]
  public partial class ModifyPlayerIcon : global::ProtoBuf.IExtensible
  {
    public ModifyPlayerIcon() {}
    
    private int _playericon;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"playericon", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int playericon
    {
      get { return _playericon; }
      set { _playericon = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"RecvEmoji")]
  public partial class RecvEmoji : global::ProtoBuf.IExtensible
  {
    public RecvEmoji() {}
    
    private int _id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int id
    {
      get { return _id; }
      set { _id = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"AccountUpgradeReq")]
  public partial class AccountUpgradeReq : global::ProtoBuf.IExtensible
  {
    public AccountUpgradeReq() {}
    
    private string _phonenumber;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"phonenumber", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string phonenumber
    {
      get { return _phonenumber; }
      set { _phonenumber = value; }
    }
    private string _password;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"password", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string password
    {
      get { return _password; }
      set { _password = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"AccountUpgradeRes")]
  public partial class AccountUpgradeRes : global::ProtoBuf.IExtensible
  {
    public AccountUpgradeRes() {}
    
    private int _status;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"status", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int status
    {
      get { return _status; }
      set { _status = value; }
    }
    private string _playeraccount = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"playeraccount", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string playeraccount
    {
      get { return _playeraccount; }
      set { _playeraccount = value; }
    }
    private string _password = "";
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"password", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string password
    {
      get { return _password; }
      set { _password = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UserLoginReq")]
  public partial class UserLoginReq : global::ProtoBuf.IExtensible
  {
    public UserLoginReq() {}
    
    private string _playeraccount;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"playeraccount", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string playeraccount
    {
      get { return _playeraccount; }
      set { _playeraccount = value; }
    }
    private string _password;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"password", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string password
    {
      get { return _password; }
      set { _password = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UserLoginRes")]
  public partial class UserLoginRes : global::ProtoBuf.IExtensible
  {
    public UserLoginRes() {}
    
    private int _errcode;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"errcode", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int errcode
    {
      get { return _errcode; }
      set { _errcode = value; }
    }
    private GuestLoginInfo _info = null;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public GuestLoginInfo info
    {
      get { return _info; }
      set { _info = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"UserLoginOutRes")]
  public partial class UserLoginOutRes : global::ProtoBuf.IExtensible
  {
    public UserLoginOutRes() {}
    
    private int _status;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"status", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int status
    {
      get { return _status; }
      set { _status = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"PlayerData")]
  public partial class PlayerData : global::ProtoBuf.IExtensible
  {
    public PlayerData() {}
    
    private int _money;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"money", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int money
    {
      get { return _money; }
      set { _money = value; }
    }
    private int _grade;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"grade", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int grade
    {
      get { return _grade; }
      set { _grade = value; }
    }
    private int _exp;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"exp", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int exp
    {
      get { return _exp; }
      set { _exp = value; }
    }
    private int _vip;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"vip", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int vip
    {
      get { return _vip; }
      set { _vip = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"LoginBonusData")]
  public partial class LoginBonusData : global::ProtoBuf.IExtensible
  {
    public LoginBonusData() {}
    
    private int _id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int id
    {
      get { return _id; }
      set { _id = value; }
    }
    private int _num;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int num
    {
      get { return _num; }
      set { _num = value; }
    }
    private int _status;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"status", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int status
    {
      get { return _status; }
      set { _status = value; }
    }
    private int _day;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"day", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int day
    {
      get { return _day; }
      set { _day = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"GetPlayerDataRes")]
  public partial class GetPlayerDataRes : global::ProtoBuf.IExtensible
  {
    public GetPlayerDataRes() {}
    
    private int _errcode;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"errcode", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int errcode
    {
      get { return _errcode; }
      set { _errcode = value; }
    }
    private PlayerData _playerdata = null;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"playerdata", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public PlayerData playerdata
    {
      get { return _playerdata; }
      set { _playerdata = value; }
    }
    private readonly global::System.Collections.Generic.List<LoginBonusData> _bonusdata = new global::System.Collections.Generic.List<LoginBonusData>();
    [global::ProtoBuf.ProtoMember(3, Name=@"bonusdata", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<LoginBonusData> bonusdata
    {
      get { return _bonusdata; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"GetLoginBonusReq")]
  public partial class GetLoginBonusReq : global::ProtoBuf.IExtensible
  {
    public GetLoginBonusReq() {}
    
    private int _signtype;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"signtype", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int signtype
    {
      get { return _signtype; }
      set { _signtype = value; }
    }
    private int _day;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"day", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int day
    {
      get { return _day; }
      set { _day = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"GetLoginBonusRes")]
  public partial class GetLoginBonusRes : global::ProtoBuf.IExtensible
  {
    public GetLoginBonusRes() {}
    
    private int _errcode;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"errcode", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int errcode
    {
      get { return _errcode; }
      set { _errcode = value; }
    }
    private int _id = default(int);
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int id
    {
      get { return _id; }
      set { _id = value; }
    }
    private int _num = default(int);
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int num
    {
      get { return _num; }
      set { _num = value; }
    }
    private readonly global::System.Collections.Generic.List<LoginBonusData> _bonusdata = new global::System.Collections.Generic.List<LoginBonusData>();
    [global::ProtoBuf.ProtoMember(4, Name=@"bonusdata", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<LoginBonusData> bonusdata
    {
      get { return _bonusdata; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    [global::ProtoBuf.ProtoContract(Name=@"Stype")]
    public enum Stype
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"INVALID_STYPE", Value=0)]
      INVALID_STYPE = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Auth", Value=1)]
      Auth = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"TalkRoom", Value=2)]
      TalkRoom = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"System", Value=3)]
      System = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Logic", Value=4)]
      Logic = 4
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"Cmd")]
    public enum Cmd
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"INVALID_CMD", Value=0)]
      INVALID_CMD = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eLoginReq", Value=1)]
      eLoginReq = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eRespose", Value=2)]
      eRespose = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eSendMsg", Value=3)]
      eSendMsg = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eSendEmoji", Value=4)]
      eSendEmoji = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eRecvMsg", Value=5)]
      eRecvMsg = 5,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eRecvEmoji", Value=6)]
      eRecvEmoji = 6,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eExitDisconnect", Value=7)]
      eExitDisconnect = 7,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eGuestLogin", Value=8)]
      eGuestLogin = 8,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eGuestLoginRes", Value=9)]
      eGuestLoginRes = 9,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eDropByOther", Value=10)]
      eDropByOther = 10,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eModifyName", Value=11)]
      eModifyName = 11,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eModifyPlayerIcon", Value=12)]
      eModifyPlayerIcon = 12,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eAccountUpgradeReq", Value=13)]
      eAccountUpgradeReq = 13,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eAccountUpgradeRes", Value=14)]
      eAccountUpgradeRes = 14,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eUserLoginReq", Value=15)]
      eUserLoginReq = 15,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eUserLoginRes", Value=16)]
      eUserLoginRes = 16,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eUserLoginOutReq", Value=17)]
      eUserLoginOutReq = 17,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eUserLoginOutRes", Value=18)]
      eUserLoginOutRes = 18,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eGetPlayerDataReq", Value=19)]
      eGetPlayerDataReq = 19,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eGetPlayerDataRes", Value=20)]
      eGetPlayerDataRes = 20,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eGetLoginBonusReq", Value=21)]
      eGetLoginBonusReq = 21,
            
      [global::ProtoBuf.ProtoEnum(Name=@"eGetLoginBonusRes", Value=22)]
      eGetLoginBonusRes = 22
    }
  
}