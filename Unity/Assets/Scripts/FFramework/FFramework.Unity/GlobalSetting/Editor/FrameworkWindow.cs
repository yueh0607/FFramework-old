using System.IO;
using System;
using UnityEditor;
using UnityEngine;

namespace FFramework.FUnityEditor
{
    public class FrameworkWindow : EditorWindow
    {

        [MenuItem("Setting", menuItem = "FFramework/MainPanel", priority = 0)]
        static void OpenWindow()
        {
            var window = GetWindow<FrameworkWindow>();
            window.name = "FFramework MainPanel";
            window.minSize = new Vector2(600, 400);
            window.Show();
        }


        Texture logo;
        private void OnEnable()
        {
            logo = LoadTextureByIO(Path.Combine(EditorHelper.ProjectPath, "FFramework/Resources/Icons/logo.png"));
        }

        
        private void OnGUI()
        {
            if(Application.isPlaying||logo==null)
            {
                Debug.LogWarning(" Main Panel not support PlayMode!(Auto close)");
                Close();
                return;
            }
            //if (logo == null) Debug.Log("null");
            //GUILayout.Label(logo, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            // 获取顶端区域的矩形范围
            Rect rect = GUILayoutUtility.GetRect(0, position.width, 0, logo.height);

            // 绘制图片
            GUI.DrawTexture(rect, logo, ScaleMode.ScaleAndCrop, true);
            string introducation = "FFramework是一款游戏开发框架";
            GUILayout.TextArea(introducation, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));


            if (GUILayout.Button("Github - FFramework"))
            {
                Application.OpenURL("https://github.com/yueh0607/FFramework");
            }

            EditorGUILayout.Space();

            RuntimeMode mode = RuntimeMode.Simulated;
#if FF_SIMULATED //模拟运行
            mode = RuntimeMode.Simulated;
#endif
#if FF_OFFLINE //单机 or 弱联网
            mode = RuntimeMode.Offline;
#endif
#if FF_HOST   //联机
            mode = RuntimeMode.Host;
#endif
            RuntimeMode last = mode;
            
            RuntimeMode rt = (RuntimeMode)EditorGUILayout.EnumPopup("Mode", mode);
            if (!EditorHelper.compiling&& last!=rt)
            {
                mode = rt;
                Debug.Log($"Mode changed to {mode}");
                switch (mode)
                {
                    case RuntimeMode.Simulated:
                        EditorHelper.AddMacro(EditorUserBuildSettings.selectedBuildTargetGroup, "FF_SIMULATED");
                        EditorHelper.RemoveMacro(EditorUserBuildSettings.selectedBuildTargetGroup, "FF_OFFLINE");
                        EditorHelper.RemoveMacro(EditorUserBuildSettings.selectedBuildTargetGroup, "FF_HOST");
                        break;
                    case RuntimeMode.Offline:
                        EditorHelper.AddMacro(EditorUserBuildSettings.selectedBuildTargetGroup, "FF_OFFLINE");
                        EditorHelper.RemoveMacro(EditorUserBuildSettings.selectedBuildTargetGroup, "FF_SIMULATED");
                        EditorHelper.RemoveMacro(EditorUserBuildSettings.selectedBuildTargetGroup, "FF_HOST");
                        break;
                    case RuntimeMode.Host:
                        EditorHelper.AddMacro(EditorUserBuildSettings.selectedBuildTargetGroup, "FF_HOST");
                        EditorHelper.RemoveMacro(EditorUserBuildSettings.selectedBuildTargetGroup, "FF_OFFLINE");
                        EditorHelper.RemoveMacro(EditorUserBuildSettings.selectedBuildTargetGroup, "FF_SIMULATED");
                        break;
                }

                if (last != mode)
                {
                    EditorHelper.NeefReCompile();
                }
            }
        }


        private Texture2D LoadTextureByIO(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            fs.Seek(0, SeekOrigin.Begin);//游标的操作，可有可无
            byte[] bytes = new byte[fs.Length];//生命字节，用来存储读取到的图片字节
            try
            {
                fs.Read(bytes, 0, bytes.Length);//开始读取，这里最好用trycatch语句，防止读取失败报错

            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            fs.Close();//切记关闭

            int width = 2048;//图片的宽（这里两个参数可以提到方法参数中）
            int height = 2048;//图片的高（这里说个题外话，pico相关的开发，这里不能大于4k×4k不然会显示异常，当时开发pico的时候应为这个问题找了大半天原因，因为美术给的图是6000*3600，导致出现切几张图后就黑屏了。。。
            Texture2D texture = new Texture2D(width, height);
            if (texture.LoadImage(bytes))
            {
                return texture;//将生成的texture2d返回，到这里就得到了外部的图片，可以使用了

            }
            else
            {
                return null;
            }
        }

    }
}

