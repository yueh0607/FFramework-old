using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace FFramework.FUnityEditor
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute showIfAttribute = attribute as ShowIfAttribute;

            // 获取被绘制的对象
            object targetObject = property.serializedObject.targetObject;

            bool condition = GetCondition(showIfAttribute.MethodName, targetObject);

            if (condition)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute showIfAttribute = attribute as ShowIfAttribute;

            // 获取被绘制的对象
            object targetObject = property.serializedObject.targetObject;

            bool condition = GetCondition(showIfAttribute.MethodName, targetObject);

            if (condition)
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }
            else
            {
                return 0f; // 隐藏字段不占用高度
            }
        }

        private bool GetCondition(string methodName, object target)
        {
            Type targetType = target.GetType();
            MethodInfo method = targetType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (method != null && method.ReturnType == typeof(bool))
            {
                return (bool)method.Invoke(target, null);
            }

            return true; // 默认返回 true
        }
    }
}