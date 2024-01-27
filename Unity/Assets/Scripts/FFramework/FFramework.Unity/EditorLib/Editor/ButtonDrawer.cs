using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;


namespace FFramework.FUnityEditor
{
    [CustomEditor(typeof(MonoBehaviour), true,isFallback =false)]
    public class ButtonMethodDrawer : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            UnityEngine.Object targetObject = target;
            MethodInfo[] methods = targetObject.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (MethodInfo method in methods)
            {
                ButtonAttribute attribute = (ButtonAttribute)method.GetCustomAttribute(typeof(ButtonAttribute));
                if(attribute != null)
                if (GUILayout.Button(attribute.buttonLabel))
                {
                    method.Invoke(targetObject, null);
                }
            }
        }
    }
}
