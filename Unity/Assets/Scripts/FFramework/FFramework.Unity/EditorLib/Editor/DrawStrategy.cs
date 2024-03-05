using FFramework.MVVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace FFramework.FUnityEditor
{
    public class DrawStrategy
    {

        public static void Draw(FieldInfo info, object obj)
        {
            if (info.FieldType == typeof(int))
            {
                info.SetValue(obj, EditorGUILayout.IntField(info.Name, (int)info.GetValue(obj)));
            }
            else if (info.FieldType == typeof(long))
            {
                info.SetValue(obj, EditorGUILayout.LongField(info.Name, (long)info.GetValue(obj)));
            }
            else if (info.FieldType == typeof(float))
            {
                info.SetValue(obj, EditorGUILayout.FloatField(info.Name, (float)info.GetValue(obj)));
            }
            else if (info.FieldType == typeof(double))
            {
                info.SetValue(obj, EditorGUILayout.DoubleField(info.Name, (double)info.GetValue(obj)));
            }
            else if (info.FieldType == typeof(string))
            {
                info.SetValue(obj, EditorGUILayout.TextField(info.Name, (string)info.GetValue(obj)));
            }
            else if (info.FieldType == typeof(bool))
            {
                info.SetValue(obj, EditorGUILayout.Toggle(info.Name, (bool)info.GetValue(obj)));
            }
            else if (info.FieldType.IsGenericType && info.FieldType.GetGenericTypeDefinition().Name.Split('`')[0] =="BindableProperty")
            {
    
                FieldInfo field = info.FieldType.GetField("_value", BindingFlags.NonPublic|BindingFlags.Instance);
                object oldValue = field.GetValue(info.GetValue(obj));
                var bindable  = info.GetValue(obj);
                Draw(field,bindable ); 
                ((IBindablePropertyNotify)bindable)?.Notify(oldValue);
            }
            else
            {
                EditorGUILayout.LabelField($"{info.Name}: Unsupported Type={info.FieldType}");
            }
        }
    }
}