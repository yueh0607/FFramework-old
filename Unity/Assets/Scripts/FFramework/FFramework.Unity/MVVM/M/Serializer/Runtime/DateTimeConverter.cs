using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework.MVVM
{
    public class DateTimeConverter : ISerializeConverter
    {
        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        public object DeSerialize(string str, Type t=null)
        {
            if (DateTime.TryParseExact(str, DateTimeFormat, null, System.Globalization.DateTimeStyles.None, out DateTime result))
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
            if (obj is DateTime dateTimeValue)
            {
                return dateTimeValue.ToString(DateTimeFormat);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
