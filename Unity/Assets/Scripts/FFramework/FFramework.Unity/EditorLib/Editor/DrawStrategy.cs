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
    

        public static void Draw(string name, object obj,Type type , Func<object,object> getter,Action<object,object> setter)
        {
            if (type == typeof(int))
            {
                setter(obj, EditorGUILayout.IntField(name, (int)getter(obj)));
            }
            else if (type == typeof(long))
            {
                setter(obj, EditorGUILayout.LongField(name, (long)getter(obj)));
            }
            else if (type == typeof(float))
            {
                setter(obj, EditorGUILayout.FloatField(name, (float)getter(obj)));
            }
            else if (type == typeof(double))
            {
                setter(obj, EditorGUILayout.DoubleField(name, (double)getter(obj)));
            }
            else if (type == typeof(string))
            {
                setter(obj, EditorGUILayout.TextField(name, (string)getter(obj)));
            }
            else if (type == typeof(bool))
            {
                setter(obj, EditorGUILayout.Toggle(name, (bool)getter(obj)));
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition().Name.Split('`')[0] == "BindableProperty")
            {
                Draw(name+"(Bindable)", getter(obj), type.GetGenericArguments()[0], (x) => ((IConvertValue)x).GetValue(), (x, y) => ((IConvertValue)x).SetValue(y));
            }
            else if(type.IsGenericType && type.GetGenericTypeDefinition().Name.Split('`')[0] == "List")
            {
                EditorGUILayout.Space(5);
                EditorGUILayout.LabelField(name+"(List)");
                object objList = getter(obj);
                IList list = (IList)objList;
                int count = list.Count;
                for(int i=0;i<count;i++)
                {
                    Draw(i.ToString(), list[i], type.GetGenericArguments()[0], (x) => ((IList)(x))[i], (x, y) => ((IList)(x))[i] = y);
                }
            }
            else if(type.FullName.EndsWith("Item"))
            {
                //公开字段
                FieldInfo[] fieldsInfoPublic = type.GetFields(BindingFlags.Instance | BindingFlags.Public |  BindingFlags.GetField);
                //非公开字段
                FieldInfo[] fieldsInfoNoPublic = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic |BindingFlags.GetField);

                foreach (var item in fieldsInfoPublic)
                {
                    DrawStrategy.Draw(item.Name, obj, item.FieldType, (x) => item.GetValue(x), (x, y) => item.SetValue(x, y));
                }
                foreach (var item in fieldsInfoNoPublic)
                {
                    DrawStrategy.Draw(item.Name, obj, item.FieldType, (x) => item.GetValue(x), (x, y) => item.SetValue(x, y));
                }

                
            }
            else
            {
                EditorGUILayout.LabelField($"{name}: Unsupported Type={type}");
            }
        }
    }
}