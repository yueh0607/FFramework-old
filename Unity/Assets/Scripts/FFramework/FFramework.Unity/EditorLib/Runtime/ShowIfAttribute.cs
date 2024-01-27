using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FFramework.FUnityEditor
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    public class ShowIfAttribute : PropertyAttribute
    {
        public string MethodName = string.Empty;

        public ShowIfAttribute(string Condition)
        {
            this.MethodName = Condition;
        }
    }
}