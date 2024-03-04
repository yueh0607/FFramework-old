using FFramework.FUnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 用于导出Block六面体贴图的工具
/// </summary>
public class BlockMapExport : EditorWindow
{
    [MenuItem("WorldTool/BlockTexture")]
    public static void OpenWindow()
    {
        GetWindow<BlockMapExport>().Show();
    }

    Texture2D[] textures = new Texture2D[6];
    int[] rots = new int[6] { 1, 1, 1, 1, 0, 0 };
    Texture2D gen;
    string name;
    private void OnGUI()
    {

        textures[5] = (Texture2D)EditorGUILayout.ObjectField("Top", textures[5], typeof(Texture2D), false);
        textures[4] = (Texture2D)EditorGUILayout.ObjectField("Bottom", textures[4], typeof(Texture2D), false);

        textures[0] = (Texture2D)EditorGUILayout.ObjectField("Side1", textures[0], typeof(Texture2D), false);
        textures[1] = (Texture2D)EditorGUILayout.ObjectField("Side2", textures[1], typeof(Texture2D), false);
        textures[2] = (Texture2D)EditorGUILayout.ObjectField("Side3", textures[2], typeof(Texture2D), false);
        textures[3] = (Texture2D)EditorGUILayout.ObjectField("Side4", textures[3], typeof(Texture2D), false);

        rots[5] = EditorGUILayout.IntSlider("TopRot", rots[5], 0, 3);
        rots[4] = EditorGUILayout.IntSlider("BottomRot", rots[4], 0, 3);
        rots[0] = EditorGUILayout.IntSlider("Side1Rot", rots[0], 0, 3);
        rots[1] = EditorGUILayout.IntSlider("Side2Rot", rots[1], 0, 3);
        rots[2] = EditorGUILayout.IntSlider("Side3Rot", rots[2], 0, 3);
        rots[3] = EditorGUILayout.IntSlider("Side4Rot", rots[3], 0, 3);



        if (GUILayout.Button("Generate"))
        {
            gen = new Texture2D(64, 64);
            SetOverride(textures[0], gen, 24, 0, rots[0]);
            SetOverride(textures[1], gen, 24, 16, rots[1]);
            SetOverride(textures[2], gen, 24, 32, rots[2]);
            SetOverride(textures[3], gen, 24, 48, rots[3]);

            SetOverride(textures[4], gen, 8, 32, rots[4]);

            SetOverride(textures[5], gen, 40, 32, rots[5]);
            gen.Apply();
        }
        EditorGUILayout.ObjectField("Generated", gen, typeof(Texture2D), false);

   
        name = EditorGUILayout.TextField("Name", name);
        DragFileToPath(ref name);
       
       

        if (GUILayout.Button("Save"))
        {
            byte[] bytes = gen.EncodeToPNG();
            File.WriteAllBytes(EditorHelper.ProjectPath+"/"+name, bytes);
            AssetDatabase.Refresh();
        }
    }


    void DragFileToPath(ref string str)
    {
        // 路径拖拽
        if (Event.current.type == EventType.DragUpdated)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            Event.current.Use();
        }
        else if (Event.current.type == EventType.DragPerform)
        {
            DragAndDrop.AcceptDrag();
            foreach (string draggedPath in DragAndDrop.paths)
            {
                str = draggedPath;
                break;
            }
            Event.current.Use();
        }

    }
    void SetOverride(Texture2D tex, Texture2D target, int xOffset, int yOffset, int rotRight)
    {
        if (tex == null || target == null) return;

        //没有旋转
        if(rotRight==0)
        {
            for (int i = 0; i < tex.width; i++)
            {
                for (int j = 0; j < tex.height; j++)
                {
                    target.SetPixel(i + xOffset, j + yOffset, tex.GetPixel(i, j));
                }
            }
        }
        else if(rotRight==1)
        {
            for (int i = 0; i < tex.width; i++)
            {
                for (int j = 0; j < tex.height; j++)
                {
                    target.SetPixel(j + xOffset, tex.width - i + yOffset, tex.GetPixel(i, j));
                }
            }
        }
        else if(rotRight==2)
        {
            for (int i = 0; i < tex.width; i++)
            {
                for (int j = 0; j < tex.height; j++)
                {
                    target.SetPixel(tex.width - i + xOffset, tex.height - j + yOffset, tex.GetPixel(i, j));
                }
            }
        }
        else if(rotRight==3)
        {
            for (int i = 0; i < tex.width; i++)
            {
                for (int j = 0; j < tex.height; j++)
                {
                    target.SetPixel(tex.height - j + xOffset, i + yOffset, tex.GetPixel(i, j));
                }
            }
        }

       
    }

}
