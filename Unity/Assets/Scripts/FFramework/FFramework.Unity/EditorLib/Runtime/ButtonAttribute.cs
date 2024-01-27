using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FFramework.FUnityEditor
{
    [AttributeUsage(AttributeTargets.Method,Inherited =true,AllowMultiple =false)]
    public class ButtonAttribute : PropertyAttribute
    {
        public string buttonLabel;

        public ButtonAttribute(string label)
        {
            buttonLabel = label;
        }
    }



}