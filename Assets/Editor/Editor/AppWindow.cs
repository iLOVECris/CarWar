using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Microsoft.Win32;
using UnityEditor;
using System.Xml;
using PlistCS;

public class AppWindow : EditorWindow {

    [MenuItem("Window/PlayerPrefs Windows")]
    static void Init()
    {
        EditorWindow.CreateInstance<AppWindow>().Show();
    }


    private const string UNIQUE_STRING = "0987654321qwertyuiopasdfghjklzxcvbnm[];,.";
    private const int UNIQUE_INT = int.MinValue;
    private const float UINQUE_FLOAT = Mathf.NegativeInfinity;
    private const string UNITY_GRAPHICS_QUALITY = "UnityGraphicsQuality";
    private const float UpdateIntervalInseconds = 1.0F;

    private bool waitTillPlistHasBeenWritten = false;
    private FileInfo tmpPlistFile;
    private List<PlayerPrefsEntry> ppeList = new List<PlayerPrefsEntry>();
    private Vector2 scrollPos;
    private string newKey = "";
    private string newValueString = "";
    private int newValueInt = 0;
    private float newValueFloat = 0;
    private float rotation = 0;
    private ValueType selectedType = ValueType.String;

    private bool showNewEntryBox = false;
    private bool isOneSelected = false;
    private bool autoRefresh = false;
    private bool sortAscending = true;

    private float oldTime = 0;
    private string _searchString = string.Empty;
    private SearchFilterType _searchFilter = SearchFilterType.All;

    private List<PlayerPrefsEntry> filteredPpeList = new List<PlayerPrefsEntry>();

    private Texture2D _refreshIcon;
    private Texture2D _deleteIcon;
    private Texture2D _addIcon;
    private Texture2D _UndoIcon;
    private Texture2D _saveIcon;


    void OnEnable()
    {
        if (!IsUnityWritingToPlist())
        {
            RefreshKeys();
        }
        EditorApplication.playmodeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playmodeStateChanged += OnPlayModeStateChanged;
    }

    private void OnPlayModeStateChanged()
    {
        waitTillPlistHasBeenWritten = IsUnityWritingToPlist();
        if (!waitTillPlistHasBeenWritten)
        {
            RefreshKeys();
        }
    }


    // Update is called once per frame
    void Update() {
        if (autoRefresh && Application.platform == RuntimePlatform.WindowsEditor && EditorApplication.isPlaying)
        {
            float newtime = Mathf.Repeat(Time.timeSinceLevelLoad, UpdateIntervalInseconds);
            if (newtime < oldTime)
            {
                RefreshKeys();
            }
            oldTime = newtime;
        }

        if (waitTillPlistHasBeenWritten)
        {
            if (new FileInfo(tmpPlistFile.FullName).Exists)
            {

            }
            else
            {
                RefreshKeys();
                waitTillPlistHasBeenWritten = false;
            }
            rotation += 0.05f;
            Repaint();

        }
        isOneSelected = false;
        foreach (PlayerPrefsEntry item in filteredPpeList)
        {
            if (item.IsSelected)
            {
                isOneSelected = true;
                break;
            }
        }


    }

    void OnGUI()
    {
        GUIStyle boldNumberFieldStyle = new GUIStyle(EditorStyles.numberField);
        boldNumberFieldStyle.font = EditorStyles.boldFont;
        GUIStyle boldToggleStyle = new GUIStyle(EditorStyles.toggle);
        boldToggleStyle.font = EditorStyles.boldFont;
        GUI.enabled = !waitTillPlistHasBeenWritten;

        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        {
            Rect optionsRect = GUILayoutUtility.GetRect(0, 20, GUILayout.ExpandWidth(false));

            if (GUILayout.Button(new GUIContent("sort  " + (sortAscending ? "△" : "△"),
                "change sorting to " + (sortAscending ? "descending" : "ascending")),
                EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
            {
                OnChangeSortModeClicked();
            }
            if (GUILayout.Button(new GUIContent("Options", "Contains additional functionality like \"Add new entry\" and \"Delete all entries\" "),
                EditorStyles.toolbarDropDown, GUILayout.ExpandWidth(false)))
            {
                GenericMenu options = new GenericMenu();
                options.AddItem(new GUIContent("New Entry..."), false, OnNewEntryClicked);
                options.AddSeparator("");
                //options.AddItem(new GUIContent("Import..."), false, OnImport);
                options.AddItem(new GUIContent("Export Selected..."), false, OnExportSelected);
                options.AddItem(new GUIContent("Export All Entries"), false, OnExportAllClicked);
                options.AddSeparator("");
                options.AddItem(new GUIContent("Delete Selected Entries"), false, OnDeleteSelectedClicked);
                options.AddItem(new GUIContent("Delete All Entries"), false, OnDeleteAllClicked);
                options.DropDown(optionsRect);
            }
            GUILayout.Space(5);
            Rect position = GUILayoutUtility.GetRect(50, 150, 10, 50, EditorStyles.toolbarTextField);
            position.width -= 16;
            position.x += 16;
            SearchString = GUI.TextField(position, SearchString, EditorStyles.toolbarTextField);

            position.x = position.x - 18;
            position.width = 20;
            if (GUI.Button(position, "", ToolbarSearchTextFieldPopup))
            {
                GenericMenu options = new GenericMenu();
                options.AddItem(new GUIContent("All"), SearchFilter == SearchFilterType.All, OnSearchAllClicked);
                options.AddItem(new GUIContent("Key"), SearchFilter == SearchFilterType.key, OnSearchKeyClicked);
                options.AddItem(new GUIContent("Value(String only)"), SearchFilter == SearchFilterType.Value, OnSearchValueClicked);
                options.DropDown(position);
            }
            position = GUILayoutUtility.GetRect(10, 10, ToolbarSearchCancelButton);
            position.x -= 5;
            if (GUI.Button(position, "", ToolbarSearchCancelButton))
            {
                SearchString = string.Empty;
            }
            GUILayout.FlexibleSpace();
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                string refreshTooltip = "Should all entris be automaticly refreshed every " + UpdateIntervalInseconds + " seconds?";
                autoRefresh = GUILayout.Toggle(autoRefresh, new GUIContent("Auto Refersh ", refreshTooltip), EditorStyles.toolbarButton,
                    GUILayout.ExpandWidth(false), GUILayout.MinWidth(75));
            }
            if (GUILayout.Button(new GUIContent(RefreshIcon, "Force a refresh,could take a few seconds "), EditorStyles.toolbarButton,
                GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
            {
                if (Application.platform == RuntimePlatform.OSXEditor)
                {
                    waitTillPlistHasBeenWritten = IsUnityWritingToPlist();
                }
                RefreshKeys();
            }
            Rect r;
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                r = GUILayoutUtility.GetRect(16, 16);
            }
            else
            {
                r = GUILayoutUtility.GetRect(9, 16);
            }

            if (waitTillPlistHasBeenWritten)
            {
                Texture2D t = AssetDatabase.LoadAssetAtPath(IconPath + "loader/" + (Mathf.FloorToInt(rotation % 12) + 1) + ".png",
                    typeof(Texture2D)) as Texture2D;
                GUI.DrawTexture(new Rect(r.x + 3, r.y, 16, 16), t);
            }


        }
        EditorGUILayout.EndHorizontal();
        GUI.enabled = !waitTillPlistHasBeenWritten;

        if (showNewEntryBox)
        {
            GUILayout.BeginHorizontal(GUI.skin.box);
            {
                GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                {
                    newKey = EditorGUILayout.TextField("key", newKey);

                    switch (selectedType)
                    {
                        default:
                        case ValueType.String:
                            newValueString = EditorGUILayout.TextField("Value", newValueString);
                            break;
                        case ValueType.Float:
                            newValueFloat = EditorGUILayout.FloatField("Value", newValueFloat);
                            break;
                        case ValueType.Integer:
                            newValueInt = EditorGUILayout.IntField("Value", newValueInt);
                            break;

                    }
                    selectedType = (ValueType)EditorGUILayout.EnumPopup("Type", selectedType);
                }
                GUILayout.EndVertical();
                GUILayout.BeginVertical(GUILayout.Width(1));
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button(new GUIContent("X", "Close"), EditorStyles.boldLabel, GUILayout.ExpandWidth(false)))
                        {
                            showNewEntryBox = false;
                        }
                    }
                    GUILayout.EndHorizontal();
                    if (GUILayout.Button(new GUIContent(AddIcon, "Add a new key-value.")))
                    {
                        if (!string.IsNullOrEmpty(newKey))
                        {
                            switch (selectedType)
                            {
                                case ValueType.Integer:
                                    PlayerPrefs.SetInt(newKey, newValueInt);
                                    ppeList.Add(new PlayerPrefsEntry(newKey, newValueInt));
                                    break;
                                case ValueType.Float:
                                    PlayerPrefs.SetFloat(newKey, newValueFloat);
                                    ppeList.Add(new PlayerPrefsEntry(newKey, newValueFloat));
                                    break;
                                default:
                                case ValueType.String:
                                    PlayerPrefs.SetString(newKey, newValueString);
                                    ppeList.Add(new PlayerPrefsEntry(newKey, newValueString));
                                    break;
                            }
                            PlayerPrefs.Save();
                            Sort();
                        }
                        newKey = newValueString = "";
                        newValueInt = 0;
                        newValueFloat = 0;
                        GUIUtility.keyboardControl = 0;
                        showNewEntryBox = false;
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.Space(2);
        GUI.backgroundColor = Color.white;
        EditorGUI.indentLevel++;

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        {
            EditorGUILayout.BeginVertical();
            {
                for (int i = 0; i < filteredPpeList.Count; i++)
                {
                    if (filteredPpeList[i].Value != null)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            filteredPpeList[i].IsSelected = GUILayout.Toggle(filteredPpeList[i].IsSelected,
                                new GUIContent("", "Toggle selection."), filteredPpeList[i].HasChanged ? boldToggleStyle : EditorStyles.toggle,
                                GUILayout.MaxWidth(20), GUILayout.MinWidth(20), GUILayout.ExpandWidth(false));
                            filteredPpeList[i].Key = EditorGUILayout.TextField(filteredPpeList[i].Key,
                                filteredPpeList[i].HasChanged ? boldNumberFieldStyle : EditorStyles.numberField, GUILayout.MaxWidth(125),
                                GUILayout.MinWidth(40), GUILayout.ExpandWidth(true));
                            GUIStyle numberFieldStyle = filteredPpeList[i].HasChanged ? boldNumberFieldStyle : EditorStyles.numberField;

                            switch (filteredPpeList[i].Type)
                            {
                                default:
                                case ValueType.String:
                                    filteredPpeList[i].Value = EditorGUILayout.TextField("", (string)filteredPpeList[i].Value,
                                        numberFieldStyle, GUILayout.MinWidth(40));
                                    break;
                                case ValueType.Float:
                                    filteredPpeList[i].Value = EditorGUILayout.FloatField("", (float)filteredPpeList[i].Value,
                                        numberFieldStyle, GUILayout.MinWidth(40));
                                    break;
                                case ValueType.Integer:
                                    filteredPpeList[i].Value = EditorGUILayout.IntField("", (int)filteredPpeList[i].Value,
                                        numberFieldStyle, GUILayout.MinWidth(40));
                                    break;
                            }

                            GUILayout.Label(new GUIContent("?", filteredPpeList[i].Type.ToString()), GUILayout.ExpandWidth(false));
                            GUI.enabled = filteredPpeList[i].HasChanged && !waitTillPlistHasBeenWritten;
                            if (GUILayout.Button(new GUIContent(SaveIcon, "Save changes made to this value."), GUILayout.ExpandWidth(false)))
                            {
                                filteredPpeList[i].SaveChanges();
                            }

                            if (GUILayout.Button(new GUIContent(UndoIcon, "Discard changes made to this value"), GUILayout.ExpandWidth(false)))
                            {
                                filteredPpeList[i].RevertChanges();
                            }
                            GUI.enabled = !waitTillPlistHasBeenWritten;

                            if (GUILayout.Button(new GUIContent(DeleteIcon, "Delete this key-value"), GUILayout.ExpandWidth(false)))
                            {
                                PlayerPrefs.DeleteKey(filteredPpeList[i].Key);
                                ppeList.Remove(filteredPpeList[i]);
                                PlayerPrefs.Save();

                                UpdateFilteredList();
                                break;

                            }

                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            EditorGUILayout.EndVertical();

        }
        EditorGUILayout.EndScrollView();
        EditorGUI.indentLevel--;
    }

    private void OnChangeSortModeClicked()
    {
        sortAscending = !sortAscending;
        Sort();
    }

    private void OnNewEntryClicked()
    {
        showNewEntryBox = true;
    }

    private void OnImport()
    {
        string importPath = EditorUtility.OpenFilePanel("Import PlayerPrefs", "", "ppe");
        if (!string.IsNullOrEmpty(importPath))
        {
            FileInfo fi = new FileInfo(importPath);
            Dictionary<string, object> plist = (Dictionary<string, object>)Plist.readPlist(fi.FullName);
            foreach (KeyValuePair<string, object> kvp in plist)
            {
                PlayerPrefsEntry entry = null;
                if (kvp.Value is float)
                    entry = new PlayerPrefsEntry(kvp.Key, (float)kvp.Value);
                else if (kvp.Value is int)
                    entry = new PlayerPrefsEntry(kvp.Key, (int)kvp.Value);
                else if (kvp.Value is string)
                    entry = new PlayerPrefsEntry(kvp.Key, (string)kvp.Value);
                if (entry != null)
                {
                    ppeList.Add(entry);
                    entry.SaveChanges();
                }


            }
            Sort();
            Repaint();

        }
    }

    private void OnExportSelected()
    {
        Export(false);
    }
    private void OnExportAllClicked()
    {
        Export(false);
    }

    private void OnDeleteSelectedClicked()
    {
        if (isOneSelected)
        {
            if (!waitTillPlistHasBeenWritten)
            {
                if (EditorUtility.DisplayDialog("Are you sure?", "Are you sure you want to delete the selected keys?There is no undo!", "Delete", "Cancel"))
                {
                    int count = filteredPpeList.Count - 1;
                    for (int i = count; i >= 0; i--)
                    {
                        if (filteredPpeList[i].IsSelected)
                        {
                            PlayerPrefs.DeleteKey(filteredPpeList[i].Key);
                            ppeList.Remove(filteredPpeList[i]);
                        }
                    }
                    PlayerPrefs.Save();
                    UpdateFilteredList();

                }
            }
            else
                Debug.Log("cannot delete PlayerPrefs entries because it is still loading");
        }
        else
            Debug.Log("Cannot delete PlayerPrefs entries because no entries has been selected");
    }

    private void OnDeleteAllClicked()
    {
        for (int i = 0; i < ppeList.Count; i++)
        {
            ppeList[i].IsSelected = true;
        }
        isOneSelected = true;
        OnDeleteSelectedClicked();
    }

    private void Export(bool onlySelected)
    {
        Dictionary<string, object> entries = new Dictionary<string, object>();
        for (int i = 0; i < filteredPpeList.Count; i++)
        {
            if (onlySelected == false)
            {
                entries.Add(this.filteredPpeList[i].Key, this.filteredPpeList[i].Value);
            }
            else if (this.filteredPpeList[i].IsSelected)
            {
                entries.Add(this.filteredPpeList[i].Key, this.filteredPpeList[i].Value);
            }
        }

        if (onlySelected && entries.Count == 0)
        {
            Debug.Log("Cannot export selected entries as no entries has been selected");
        }
        else
        {
            string exportPath = EditorUtility.SaveFilePanelInProject("Export all PlayerPrefs entries", PlayerSettings.productName +
                "_PlayerPrefs", "ppe", "Export all PlayerPrefs entries");

            if (!string.IsNullOrEmpty(exportPath))
            {
                string xml = Plist.writeXml(entries);
                File.WriteAllText(exportPath, xml);
                AssetDatabase.Refresh();
            }
        }
    }

    private void OnSearchAllClicked()
    {
        SearchFilter = SearchFilterType.All;
    }
    private void OnSearchKeyClicked()
    {
        SearchFilter = SearchFilterType.key;
    }

    private void OnSearchValueClicked()
    {
        SearchFilter = SearchFilterType.Value;
    }

    private void UpdateFilteredList()
    {
        filteredPpeList.Clear();
        if (!string.IsNullOrEmpty(SearchString))
        {
            for (int i = 0; i < ppeList.Count; i++)
            {
                if (SearchFilter == SearchFilterType.key || _searchFilter == SearchFilterType.All)
                {
                    if (ppeList[i].Key.ToLowerInvariant().Contains(SearchString.Trim().ToLowerInvariant()))
                    {
                        filteredPpeList.Add(ppeList[i]);
                    }
                }

                if ((SearchFilter == SearchFilterType.Value || SearchFilter == SearchFilterType.All) && ppeList[i].Type == ValueType.String)
                {
                    if (!filteredPpeList.Contains(ppeList[i]))
                    {
                        if (((string)ppeList[i].Value).ToLowerInvariant().Contains(SearchString.Trim().ToLowerInvariant()))
                        {
                            filteredPpeList.Add(ppeList[i]);
                        }
                    }
                }



            }
        }
        else
        {
            filteredPpeList.AddRange(ppeList);
        }
    }

    private void Sort()
    {
        if (sortAscending)
        {
            ppeList.Sort(PlayerPrefsEntry.SortByNameAscending);
        }
        else
        {
            ppeList.Sort(PlayerPrefsEntry.SortByNameDescending);
        }

        UpdateFilteredList();
    }

    private void RefreshKeys()
    {
        ppeList.Clear();
        string[] allKeys = GetAllKeys();
        for (int i = 0; i < allKeys.Length; i++)
        {
            ppeList.Add(new PlayerPrefsEntry(allKeys[i]));
        }
        Sort();
        Repaint();

    }

    private string[] GetAllKeys()
    {
        List<string> result = new List<string>();
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            result.AddRange(GetAllWindowsKeys());
        }
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            result.AddRange(GetAllMacKeys());
        }
#if UNITY_EDITOR_LINUX
        else if(Application.platform==RuntimePlatform.LinuxEditor)
        {
            result.AddRange(GetAllLinuxKeys());
        }
#endif
        else
        {
            Debug.LogError("Unsupported platform detected,please contact support@rejected-games.com and let us knows");
        }
        if (result.Contains(UNITY_GRAPHICS_QUALITY))
        {
            result.Remove(UNITY_GRAPHICS_QUALITY);
        }
        return result.ToArray();
    }

    private string[] GetAllMacKeys()
    {
        string plistPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Preferences/unity." +
            PlayerSettings.companyName + "." + PlayerSettings.productName + ".plist";
        string[] keys = new string[0];
        if (File.Exists(plistPath))
        {
            FileInfo fi = new FileInfo(plistPath);
            Dictionary<string, object> plist = (Dictionary<string, object>)Plist.readPlist(fi.FullName);
            keys = new string[plist.Count];
            plist.Keys.CopyTo(keys, 0);

        }
        return keys;
    }

    private string[] GetAllWindowsKeys()
    {
        RegistryKey cuKey = Registry.CurrentUser;

        RegistryKey unityKey = cuKey.CreateSubKey("Software\\Unity\\UnityEditor\\" + PlayerSettings.companyName + "\\" + PlayerSettings.productName);
        string[] values = unityKey.GetValueNames();
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = values[i].Substring(0, values[i].LastIndexOf("_"));
        }
        return values;
    }

    private string[] GetAllLinuxKeys()
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/.config/unity3d/" + PlayerSettings.companyName +
            "/" + PlayerSettings.productName + "/prefs";
        List<string> keys = new List<string>();
        XmlDocument xmlDoc = new XmlDocument();

        if (System.IO.File.Exists(path))
        {
            xmlDoc.LoadXml(System.IO.File.ReadAllText(path));
        }
        foreach (XmlElement node in xmlDoc.SelectNodes("unity_prefs/pref"))
        {
            keys.Add(node.GetAttribute("name"));
        }
        return keys.ToArray();
    }


    private bool IsUnityWritingToPlist()
    {
        bool result = false;
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            FileInfo plistFile = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Preferences/unity." + PlayerSettings.companyName + "." + PlayerSettings.productName + ".plist");
            DirectoryInfo di = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Preferences/");
            FileInfo[] allPreferenceFiles = di.GetFiles();
            foreach (FileInfo fi in allPreferenceFiles)
            {
                if (fi.FullName.Contains(tmpPlistFile.FullName))
                {
                    if (!fi.FullName.EndsWith(".plist"))
                    {
                        tmpPlistFile = fi;
                        result = true;
                    }
                }
            }
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            result = false;
        }
        return result;
    }


    private string SearchString
    {
        get
        {
            return _searchString;
        }
        set
        {
            if (_searchString != value)
            {
                _searchString = value;
                UpdateFilteredList();
            }

        }       
    }

    private SearchFilterType SearchFilter
    {
        get { return _searchFilter; }
        set
        {
            if (_searchFilter != value)
            {
                _searchFilter = value;
                UpdateFilteredList();
            }
        }


    }

    private class PlayerPrefsEntry
    {
        private string key;
        private object value;

        public ValueType Type;
        public bool IsSelected = false;
        public bool HasChanged = false;
        private string oldKey;

        public PlayerPrefsEntry(string key)
        {
            this.key = key;
            oldKey = key;
            RetrieveValue();
        }

        public PlayerPrefsEntry(string key, string value)
        {
            this.key = key;
            this.value = value;
            this.Type = ValueType.String;
        }
        public PlayerPrefsEntry(string key, float value)
        {
            this.key = key;
            this.value = value;
            this.Type = ValueType.Float;
        }

        public PlayerPrefsEntry(string key, int value)
        {
            this.key = key;
            this.value = value;
            this.Type = ValueType.Integer;
        }

        public void SaveChanges()
        {
            switch (Type)
            {
                default:
                case ValueType.String:
                    PlayerPrefs.SetString(Key, (string)value);
                    break;
                case ValueType.Float:
                    PlayerPrefs.SetFloat(Key, (float)value);
                    break;
                case ValueType.Integer:
                    PlayerPrefs.SetInt(Key, (int)value);
                    break;
            }
            if (oldKey != Key)
            {
                PlayerPrefs.DeleteKey(oldKey);
                oldKey = Key;
            }
            HasChanged = false;
            PlayerPrefs.Save();
        }
        public void RevertChanges()
        {
            RetrieveValue();
        }

        public void RetrieveValue()
        {
            key = oldKey;
            if (PlayerPrefs.GetString(Key, UNIQUE_STRING) != UNIQUE_STRING)
            {
                Type = ValueType.String;
                value = PlayerPrefs.GetString(Key);
            }
            else if (PlayerPrefs.GetInt(Key, UNIQUE_INT) != UNIQUE_INT)
            {
                Type = ValueType.Integer;
                value = PlayerPrefs.GetInt(Key);
            }
            else if (PlayerPrefs.GetFloat(Key, UINQUE_FLOAT) != UINQUE_FLOAT)
            {
                Type = ValueType.Float;
                value = PlayerPrefs.GetFloat(Key);
            }
            oldKey = Key;
            HasChanged = false;

        }

        public string Key
        {
            get
            {
                return key;
            }

            set
            {
                if (value != key)
                {
                    HasChanged = true;
                    key = value;
                }
            }
        }

        public object Value
        {
            get
            {
                return this.value;
            }
            set
            {
                if (!value.Equals(this.value))
                {
                    this.value = value;
                    HasChanged = true;
                }
            }
        }

        public static int SortByNameAscending(PlayerPrefsEntry a, PlayerPrefsEntry b)
        {
            return string.Compare(a.Key, b.Key);
        }

        public static int SortByNameDescending(PlayerPrefsEntry a, PlayerPrefsEntry b)
        {
            return string.Compare(b.Key, a.Key);
        }

    }
    private enum ValueType
    {
        String,
        Float,
        Integer
    }

    private enum SearchFilterType
    {
        All,
        key,
        Value
    }

    public static void Separator(Color color)
    {
        Color old = GUI.color;
        GUI.color = color;
        Rect lineRect = GUILayoutUtility.GetRect(10, 1);
        GUI.DrawTexture(new Rect(lineRect.x, lineRect.y, lineRect.width, 1), EditorGUIUtility.whiteTexture);
        GUI.color = old;
    }

    public Texture2D DeleteIcon
    {
        get
        {
            if ((UnityEngine.Object)_deleteIcon == (UnityEngine.Object)null)
            {
                _deleteIcon = AssetDatabase.LoadAssetAtPath(IconPath + "delete.png", typeof(Texture2D)) as Texture2D;
            }
            return _deleteIcon;
        }
    }

    public Texture2D AddIcon
    {
        get
        {
            if ((UnityEngine.Object)_addIcon == (UnityEngine.Object)null)
            {
                _addIcon = AssetDatabase.LoadAssetAtPath(IconPath + "add.png", typeof(Texture2D)) as Texture2D;
            }
            return _addIcon;
        }
    }
    public Texture2D UndoIcon
    {
        get
        {
            if((UnityEngine.Object)_UndoIcon==(UnityEngine.Object)null)
            {
                _UndoIcon = AssetDatabase.LoadAssetAtPath(iconsPath + "undo.png", typeof(Texture2D)) as Texture2D;
            }
            return _UndoIcon;
        }
    }
    public Texture2D SaveIcon
    {
        get
        {
            if ((UnityEngine.Object)_saveIcon == (UnityEngine.Object)null)
            {
                _saveIcon = AssetDatabase.LoadAssetAtPath(IconPath + "save.png", typeof(Texture2D)) as Texture2D;
            }
            return _saveIcon;
        }
    }

    public Texture2D RefreshIcon
    {
        get
        {
            if ((UnityEngine.Object)_refreshIcon == (UnityEngine.Object)null)
            {
                _refreshIcon = AssetDatabase.LoadAssetAtPath(IconPath + "refresh.png", typeof(Texture2D)) as Texture2D;
            }
            return _refreshIcon;
        }
    }

    private string iconsPath;
    private string IconPath
    {
        get
        {
            if (string.IsNullOrEmpty(iconsPath))
            {
                string path = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));
                path = path.Substring(0, path.LastIndexOf('/'));
                path = path.Substring(0, path.LastIndexOf('/') + 1);
                iconsPath = path + "Icons/";

            }
            return iconsPath;
        }
    }

    private GUIStyle ToolbarSearchField
    {
        get
        {
            return GetStyle("ToolbarSeachTextField");
        }
    }


    private GUIStyle ToolbarSearchTextFieldPopup
    {
        get
        {
            return GetStyle("ToolbarSeachTextFieldPopup");
        }
    }
    private GUIStyle ToolbarSearchCancelButton
    {
        get
        {
            return GetStyle("ToolbarSeachCancelButton");
        }
    }
    private GUIStyle ToolbarSearchCancelButtonEmpty
    {
        get
        {
            return GetStyle("ToolbarSeachCancelButtonEmpty");
        }
    }


    private GUIStyle GetStyle(string styleName)
    {
        GUIStyle guiStyle = GUI.skin.FindStyle(styleName) ?? EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).FindStyle(styleName);

        if(guiStyle==null)
        {
            Debug.LogError((object)("Missing build-in guistyle " + styleName));
            guiStyle = GUI.skin.button;
        }
        return guiStyle;
    }
    
}
