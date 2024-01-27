using UnityEditor;
using UnityEngine;

namespace FFramework.MVVM.UnityEditor
{
    [FilePath("FFramework/RefBuilderSettings.asset", FilePathAttribute.Location.PreferencesFolder)]
    public class RefBuilderSettings : ScriptableSingleton<RefBuilderSettings>
    {
        public string defaultNameSpace = "FFramework.MVC.RefCache";
        public string defaultPath = "Assets/Scripts/Project/RefCache";
        public bool increase = false;
        public bool part = false;
        public bool autoCreatePath = true;
        public bool awakrInit = false;
        public string baseClass = "FFramework.MVVM.View";
        public string binderName = "FFramework.MVVM.BindableProperty";
        public void Modify()
        {
            Save(true);
            //Debug.Log($"{nameof(RefBuilderSettings)} has saved to: " + GetFilePath());
        }
    }
}