using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace FFramework.MVVM
{
    public class BindableConverter : ISerializeConverter
    {
        public object DeSerialize(string str,Type type=null)
        {
            object obj = Activator.CreateInstance(type,true);
            FieldInfo valueField = obj.GetType().GetField("_value", BindingFlags.NonPublic | BindingFlags.Instance);
            valueField.SetValue(obj, ModelSerialize.DeSerialize(str, valueField.FieldType));
            return obj;
        }

        public string Serialize(object obj)
        {
            //对应的类型
            FieldInfo valueField = obj.GetType().GetField("_value", BindingFlags.NonPublic | BindingFlags.Instance);
            return ModelSerialize.Serialize(valueField.GetValue(obj));
        }
    }

}
