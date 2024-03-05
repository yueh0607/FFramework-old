using Cinemachine.Utility;
using FFramework.MVVM;
using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace FFramework.FUnityEditor
{
    [CustomEditor(typeof(ModelView))]
    class ModelViewInspector : Editor
    {
        SerializedProperty selectedModels;
        SerializedProperty inAssembly;
        SerializedProperty openState;
        private void OnEnable()
        {
          
            selectedModels = serializedObject.FindProperty($"{nameof(ModelView.models)}");
            inAssembly = serializedObject.FindProperty($"{nameof(ModelView.assemblyName)}");
            openState = serializedObject.FindProperty(nameof(ModelView.open));
        }
        Dictionary<long, bool> foldouts = new Dictionary<long, bool>();

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("只有在运行时才能读取Model数据", MessageType.Info);
                return;
            }
            if (!openState.boolValue)
            {
                EditorGUILayout.HelpBox("出于性能考虑，只有在Open状态的ModelView才会绘制", MessageType.Info);
                return;
            }


            //反射所有程序集
            Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            // 使用 LINQ 查询获取特定名称的程序集
            Assembly targetAssembly = loadedAssemblies.FirstOrDefault(assembly => assembly.FullName.StartsWith(inAssembly.stringValue));

            if (selectedModels.stringValue == "")
            {
                EditorGUILayout.HelpBox($"Model名称不能为空", MessageType.Warning);
                return;
            }
            //取得指定类型
            Type type = targetAssembly?.GetType(selectedModels.stringValue);
            if (type == null)
            {
                EditorGUILayout.HelpBox($"在指定程序集不存在该Model类型或不存在该程序集 AssemblyNull={targetAssembly == null}", MessageType.Error);
                return;
            }
            GUILayout.Space(10);
            GUILayout.Label("Model视图");

            //公开字段
            FieldInfo[] fieldsInfoPublic = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            //非公开字段
            FieldInfo[] fieldsInfoNoPublic = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            var models = MV.GetModels(type);

 
            if (models.Count==0)
            {
                EditorGUILayout.HelpBox($"此类型没有Model已经实例化", MessageType.Info);
                return;
            }
            foreach(var kvp in models)
            {
                if (!foldouts.ContainsKey(kvp.Key)) foldouts.Add(kvp.Key,false);
                foldouts[kvp.Key] = EditorGUILayout.BeginFoldoutHeaderGroup(foldouts[kvp.Key], $"ID={kvp.Key}");

                if (foldouts[kvp.Key])
                {
                    EditorGUILayout.LabelField("Public Fields");
                    foreach (var item in fieldsInfoPublic)
                    {
                        DrawStrategy.Draw(item,kvp.Value);
                    }
                    EditorGUILayout.LabelField("Private Fields");
                    foreach (var item in fieldsInfoNoPublic)
                    {
                        DrawStrategy.Draw(item, kvp.Value);
                    }
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
            
            serializedObject.ApplyModifiedProperties();
        }


    }
}