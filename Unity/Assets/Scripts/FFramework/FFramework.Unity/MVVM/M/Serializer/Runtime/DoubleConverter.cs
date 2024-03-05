using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FFramework.MVVM
{
    public class DoubleConverter : ISerializeConverter
    {
        public object DeSerialize(string str, Type t = null)
        {
            if (double.TryParse(str, out double result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public string Serialize(object obj)
        {
            if (obj is double doubleValue)
            {
                return doubleValue.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }

    public class FloatConverter : ISerializeConverter
    {
        public object DeSerialize(string str, Type t = null)
        {
            if (float.TryParse(str, out float result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        public string Serialize(object obj)
        {
            if (obj is float floatValue)
            {
                return floatValue.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
