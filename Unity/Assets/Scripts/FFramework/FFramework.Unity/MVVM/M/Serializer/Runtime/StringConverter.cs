using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework.MVVM
{
    public class StringConverter : ISerializeConverter
    {
        public object DeSerialize(string str, Type t = null)
        {
            return str;
        }

        public string Serialize(object obj)
        {
            return obj?.ToString() ?? string.Empty;
        }
    }

   
}