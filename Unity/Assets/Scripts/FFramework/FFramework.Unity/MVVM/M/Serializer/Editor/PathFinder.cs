using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FFramework.FUnityEditor
{
    public class PathFinder
    {
        [MenuItem(itemName: "StreamingAssets", menuItem ="FFramework/OpenPath/StreamingAssets Path")]
        static void Open1()
        {
            OpenURL(Application.streamingAssetsPath);
        }
        [MenuItem(itemName: "PeristentData", menuItem = "FFramework/OpenPath/PersistentData Path")]
        static void Open2()
        {
            OpenURL(Application.persistentDataPath);
        }
        [MenuItem(itemName: "Data", menuItem = "FFramework/OpenPath/Data Path")]
        static void Open3()
        {
            OpenURL(Application.dataPath);
        }

        [MenuItem(itemName: "Project", menuItem = "FFramework/OpenPath/Project Path")]
        static void Open4()
        {
            OpenURL(EditorHelper.ProjectPath);
        }

        [MenuItem(itemName: "UnityEditor", menuItem = "FFramework/OpenPath/UnityEditor Path")]
        static void Open5()
        {
            OpenURL(EditorApplication.applicationPath);
        }
        [MenuItem(itemName: "EditorContents", menuItem = "FFramework/OpenPath/EditorContents Path")]
        static void Open6()
        {
            OpenURL(EditorApplication.applicationContentsPath);
        }
        [MenuItem(itemName: "ConsoleLog", menuItem = "FFramework/OpenPath/ConsoleLog Path")]
        static void Open7()
        {
            OpenURL(Application.consoleLogPath);
        }
        [MenuItem(itemName: "TempCahce", menuItem = "FFramework/OpenPath/TemporaryCache Path")]
        static void Open8()
        {
            OpenURL(Application.temporaryCachePath);
        }

        static void OpenURL(string path)
        {
            Application.OpenURL(path);
        }
    }
}
