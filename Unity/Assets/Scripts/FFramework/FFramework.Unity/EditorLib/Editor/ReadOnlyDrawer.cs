using UnityEditor;
using UnityEngine;


namespace FFramework.FUnityEditor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class DisplayOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);

            GUI.enabled = true;
            // string value = "";
            //  switch (property.propertyType)
            //   {
            //     case SerializedPropertyType.Integer:
            //         value = property.intValue.ToString();
            //         break;
            //  }

            //   EditorGUI.LabelField(position, property.name + "\t：" + value);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}