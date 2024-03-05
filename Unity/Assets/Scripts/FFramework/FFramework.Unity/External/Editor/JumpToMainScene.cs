using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class AutoSwitchScene : MonoBehaviour
{
    // 在编辑器加载时注册回调
    static AutoSwitchScene()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }


    static string lastScene = string.Empty;
    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        //Debug.Log($"State:{state}-----------------------------------------------");
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            // 在进入播放模式前保存当前场景e
            if(!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }
            lastScene = EditorSceneManager.GetActiveScene().path;
            EditorPrefs.SetString("FF-Last Scene", lastScene);
            // 在进入播放模式时切换场景
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
           
            //Debug.Log($"Last {lastScene}");
            //EditorSceneManager.OpenScene(scene.path,OpenSceneMode.Single);

        }
        else if (state == PlayModeStateChange.EnteredEditMode)
        {
            lastScene = EditorPrefs.GetString("FF-Last Scene");
            // 在退出播放模式时切换回原场景
            if (lastScene != string.Empty)
            {
                //Debug.Log($"Ret -> {lastScene}");
                EditorSceneManager.OpenScene(lastScene,OpenSceneMode.Single);
            }
            lastScene = string.Empty;
        }
    }

}