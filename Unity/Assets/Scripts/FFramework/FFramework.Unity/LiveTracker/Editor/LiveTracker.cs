using FFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LiveTracker : EditorWindow
{

    [MenuItem("LiveTracker", menuItem = "FFramework/LiveTracker")]
    static void OpenWindow()
    {
        var window = GetWindow<LiveTracker>();
        window.Show();



    }

    Vector2 scrollPosition = Vector2.zero;


    bool hasUpdate = false;
    private void OnFocus()
    {
        if (Application.isPlaying)
        {
            if (!hasUpdate)
            {
                hasUpdate = true;
                //UnityGlobal.MainThread.UpdateThread.FrameCall += (x) => this.Repaint();
            }
        }
        else
        {
            hasUpdate = false;
        }
    }

    float mouseY = 0;
    bool hideInPool = true;
    bool hideDeconstructed = true;
    void OnGUI()
    {
        this.titleContent = new GUIContent($"LiveTracker-{(hasUpdate ? "Runtime" : "Editor")}");
        var weaks = FUnit.GetWeakReferences();

        float width = this.position.size.x;
        float cell1 = width * 0.25f;
        float cell2 = width * 0.6f;
        float cell3 = width * 0.15f;

        GUILayout.BeginHorizontal();
        hideInPool = GUILayout.Toggle(hideInPool, new GUIContent("HideInPool"));
        hideDeconstructed = GUILayout.Toggle(hideDeconstructed, new GUIContent("HideDeconstructed"));
        GUILayout.EndHorizontal();

        var startY = GUILayoutUtility.GetLastRect().max.y;

        var lastRect = GUILayoutUtility.GetLastRect();
        Handles.DrawLine(new Vector3(0, lastRect.max.y, 0), new Vector3(width, lastRect.max.y, 0), 0.3f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("ID", GUILayout.Width(cell1));
        GUILayout.Label("Type", GUILayout.Width(cell2));
        GUILayout.Label("InPool", GUILayout.Width(cell3));
        GUILayout.EndHorizontal();


        lastRect = GUILayoutUtility.GetLastRect();

        Handles.DrawLine(new Vector3(0, lastRect.max.y, 0), new Vector3(width, lastRect.max.y, 0), 0.5f);
      
        if (Event.current.type == EventType.MouseDown)
        {
            mouseY = Event.current.mousePosition.y;
            Repaint();
        }
        if (mouseY > 0.01f)
        {

            Handles.color = Color.green;
            Handles.DrawLine(new Vector3(0, mouseY, 0), new Vector3(width, mouseY, 0), 0.3f);
            Handles.color = Color.white;
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        foreach (var weak in weaks)
        {
            try
            {
                FUnit unit = (FUnit)weak.Target;
                if (hideInPool && unit.IsInPool) continue;
                GUILayout.BeginHorizontal();
                GUILayout.Label(unit.ID.ToString(), GUILayout.Width(cell1));
                GUILayout.Label(unit.GetType().GetDisplayName(), GUILayout.Width(cell2));
                GUILayout.Label(unit.IsInPool.ToString(), GUILayout.Width(cell3));
                GUILayout.EndHorizontal();
            }
            catch
            {
                continue;
            }
        }
        if (!hideDeconstructed)
        {
            var constructs = FUnit.GetConstructs();
            foreach (var con in constructs)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(con.id.ToString(), GUILayout.Width(cell1));
                GUILayout.Label(con.type, GUILayout.Width(cell2));
                GUILayout.Label("Deconstructed", GUILayout.Width(cell3));
                GUILayout.EndHorizontal();
            }
        }


        EditorGUILayout.EndScrollView();

        float endY = GUILayoutUtility.GetLastRect().max.y;

        Handles.DrawLine(new Vector3(cell1, startY, 0), new Vector3(cell1, endY, 0), 0.3f);
        Handles.DrawLine(new Vector3(cell1 + cell2, startY, 0), new Vector3(cell1 + cell2, endY, 0), 0.3f);

    }
}
