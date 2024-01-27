using UnityEditor;

namespace AirEditor
{
    [FilePath("FFramework/Settings/GlobalSettings.asset",FilePathAttribute.Location.ProjectFolder)]
    public class FrameworkSettings : ScriptableSingleton<FrameworkSettings>
    {
        public string authorName = "#unknown#";
        public string defaultNamespace = "MyNamespace";

    }
}
