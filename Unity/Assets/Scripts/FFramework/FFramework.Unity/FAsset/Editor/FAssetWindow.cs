using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//不会写，跑了跑了
namespace FFramework.FUnityEditor
{
    public class FAssetWindow : EditorWindow
    {

        //[MenuItem("Setting", menuItem = "FFramework/AssetBundle", priority = 100)]
        static void OpenWindow()
        {
            var window = GetWindow<FAssetWindow>();
            window.name = "FAsset";
            window.minSize = new Vector2(600, 400);
            window.Show();
        }

        private void OnGUI()
        {
            
        }
    }
}

