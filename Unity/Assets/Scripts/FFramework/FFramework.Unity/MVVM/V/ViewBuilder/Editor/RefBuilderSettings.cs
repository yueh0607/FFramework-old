using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace FFramework.MVVM.UnityEditor
{
    [FilePath("FFramework/RefBuilderSettings.asset", FilePathAttribute.Location.PreferencesFolder)]
    public class RefBuilderSettings : ScriptableSingleton<RefBuilderSettings>
    {
        public string defaultNameSpace = "FFramework.MVVM.RefCache";
        public string defaultPath = "Assets/Scripts/Project/RefCache";
        public bool increase = false;
        public bool part = false;
        public bool autoCreatePath = true;
        public bool awakeInit = false;
        public bool isUI = false;
        public string binderName = "FFramework.MVVM.BindableProperty";
        public bool generateViewModel = false;
        public string viewModelPath = "Assets/Scripts/Project/ViewModels";
        public bool notVMOverride = true;
        public void Modify()
        {
            Save(true);
            //Debug.Log($"{nameof(RefBuilderSettings)} has saved to: " + GetFilePath());
        }
    }
}