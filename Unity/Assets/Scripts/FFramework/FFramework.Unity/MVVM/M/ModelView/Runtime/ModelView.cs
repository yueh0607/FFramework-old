using UnityEngine;

namespace FFramework.FUnityEditor
{
    public class ModelView : MonoBehaviour
    {
#if UNITY_EDITOR
        public string models="";
        public string assemblyName="Assembly-CSharp";
        public bool open = true;
      
#endif

    }
}
