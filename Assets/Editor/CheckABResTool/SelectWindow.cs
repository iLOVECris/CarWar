using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SelectWindow : EditorWindow {
    ParserABFile parser { get; set;}
    public static bool IsNeedReLoad { get; set; }
    Vector2 ResListScroll { get; set; }

    string SelectABName = string.Empty;
    string Filter_Text = string.Empty;

    private void OnGUI()
    {
        if(parser==null||IsNeedReLoad)
        {
            IsNeedReLoad = false;
            parser = new ParserABFile();
        }
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(SelectABName, GUILayout.Width(180f));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Select",GUILayout.Width(80f),GUILayout.Height(40)))
        {
            SelectABName = parser.LoadFile();
        }
        EditorGUILayout.EndHorizontal();
        if(string.IsNullOrEmpty(SelectABName))
        {
            return;
        }

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ResType", GUILayout.Width(60f));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        parser.SelectedResType = EditorGUILayout.Popup(parser.SelectedResType, parser.FilterArray);
        EditorGUILayout.EndHorizontal();

        if(parser.SelectedResType ==-1)
        {
            return;
        }

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ResName");
        EditorGUILayout.EndHorizontal();

        ResListScroll = EditorGUILayout.BeginScrollView(ResListScroll);
        Filter_Text = EditorGUILayout.TextArea(Filter_Text, GUILayout.Width(position.width - 10), GUILayout.Height(250));
        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal();
        if(string.IsNullOrEmpty(Filter_Text))
        {
            return;
        }
        if (GUILayout.Button("start", GUILayout.Width(120f), GUILayout.Height(65)))
        {
            parser.ParserTextAreaText(Filter_Text);
        }
        EditorGUILayout.EndHorizontal();

    }

}
