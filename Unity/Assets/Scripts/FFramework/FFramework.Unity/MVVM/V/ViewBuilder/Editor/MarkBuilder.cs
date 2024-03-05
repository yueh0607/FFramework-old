using FFramework.FUnityEditor;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace FFramework.MVVM.UnityEditor
{
    public class MarkBuilder : EditorWindow
    {


        [MenuItem("FFramework", menuItem = "FFramework/ViewBuilder")]
        static void Open()
        {
            var window = GetWindow<MarkBuilder>();
            window.Show();
        }

        GameObject tar;
        GameObject target
        {
            get
            {
                return tar;
            }
            set
            {
                tar = value;
                className = tar == null ? "----------------------------" : tar.name;
            }
        }



        string path = string.Empty;
        bool increase = false;
        bool part = false;
        string nameSpaceName = "";
        string className = "----------------------------";
        bool autoCreatePath = false;
        private string binderName = "BindableProperty";
        bool awakeInit = false;
        bool isUI;
        bool generateViewModel = false;
        string vmGenPath = "";
        bool notVMOverride = true;
        private void OnEnable()
        {
            path = RefBuilderSettings.instance.defaultPath;
            increase = RefBuilderSettings.instance.increase;
            part = RefBuilderSettings.instance.part;
            nameSpaceName = RefBuilderSettings.instance.defaultNameSpace;
            autoCreatePath = RefBuilderSettings.instance.autoCreatePath;
            awakeInit = RefBuilderSettings.instance.awakeInit;
            isUI = RefBuilderSettings.instance.isUI;
            binderName = RefBuilderSettings.instance.binderName;
            generateViewModel = RefBuilderSettings.instance.generateViewModel;
            vmGenPath = RefBuilderSettings.instance.viewModelPath;
            notVMOverride = RefBuilderSettings.instance.notVMOverride;
        }
        private void OnDisable()
        {
            RefBuilderSettings.instance.defaultPath = path;
            RefBuilderSettings.instance.increase = increase;
            RefBuilderSettings.instance.part = part;
            RefBuilderSettings.instance.defaultNameSpace = nameSpaceName;
            RefBuilderSettings.instance.autoCreatePath = autoCreatePath;
            RefBuilderSettings.instance.awakeInit = awakeInit;
            RefBuilderSettings.instance.isUI = isUI;
            RefBuilderSettings.instance.binderName = binderName;
            RefBuilderSettings.instance.generateViewModel = generateViewModel;
            RefBuilderSettings.instance.viewModelPath = vmGenPath;
            RefBuilderSettings.instance.notVMOverride = notVMOverride;

            RefBuilderSettings.instance.Modify();
            EditorUtility.SetDirty(RefBuilderSettings.instance);
            AssetDatabase.SaveAssetIfDirty(RefBuilderSettings.instance);
        }

        void BuildTarget()
        {
            //参数检查
            if (target == null)
            {
                EditorUtility.DisplayDialog("Target Error", "Target cannot be null", "OK");
                return;
            }
            if (!autoCreatePath)
                if (Path.HasExtension(path))
                {
                    EditorUtility.DisplayDialog("Path Error", "Files with extension names cannot be used as a directory for storing reference scripts", "OK");
                    return;
                }
            if (!autoCreatePath)
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    EditorUtility.DisplayDialog("Path Error", "Please enter a valid path that exists to store the built reference script", "OK");
                    return;
                }

            //路径合成
            string filePath = Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length)
                + path.TrimEnd('/') + "/" + className + ".cs";
            //构建写入
            Build(filePath, target, nameSpaceName, className, increase, part);
            //导入资源
            //AssetDatabase.ImportAsset(filePath);
            AssetDatabase.Refresh();
            Debug.Log($"Build \"{target.name}\" Ref Completed");
        }
        private void OnGUI()
        {
            var tempT = (GameObject)EditorGUILayout.ObjectField("BuildTarget", target, typeof(GameObject), true);

            target = tempT;

            //生成路径
            path = EditorGUILayout.TextField("生成到路径", path);
            //路径不存在时，是否创建此路径
            autoCreatePath = EditorGUILayout.Toggle("不存在时是否创建", autoCreatePath);
            className = EditorGUILayout.TextField("类名(自动生成)", className);
            isUI = EditorGUILayout.Toggle("是UI面板", isUI);
            nameSpaceName = EditorGUILayout.TextField("命名空间", nameSpaceName);
            binderName = EditorGUILayout.TextField("绑定器类名", binderName);
            //一个物体有多个相同组件时，开启增量生成将使得组件名唯一，但是名字会更长
            increase = EditorGUILayout.Toggle("强化名称生成", increase);
            //表明将引用生成为分部类，不会自动调用初始化，否则生成为Mono
            part = EditorGUILayout.Toggle("是分部类", part);
            //表明是否生成Awake自动调用
            awakeInit = EditorGUILayout.Toggle("Awake时初始化", awakeInit);
            generateViewModel = EditorGUILayout.Toggle("生成ViewModel", generateViewModel);
            if (generateViewModel)
            {
                vmGenPath = EditorGUILayout.TextField("ViewModel生成路径", vmGenPath);
                notVMOverride = EditorGUILayout.Toggle("ViewModel禁用覆盖", notVMOverride);
            }



            // 路径拖拽
            if (Event.current.type == EventType.DragUpdated)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                Event.current.Use();
            }
            else if (Event.current.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                foreach (string draggedPath in DragAndDrop.paths)
                {
                    path = draggedPath;
                    break;
                }
                Event.current.Use();
            }

            if (EditorGUILayout.BeginFoldoutHeaderGroup(true, "Build Ref（Cannot Foldout）"))
            {
                if (GUILayout.Button("Build Target"))
                {
                    if (!notVMOverride)
                    {
                        if (!EditorUtility.DisplayDialog("ViewModel Override", "Are you sure to override the exist ViewModel?", "Yes", "No"))
                        {
                            return;
                        }

                    }

                    BuildTarget();
                }
                if (GUILayout.Button("Build In Scene"))
                {

                    if (!notVMOverride)
                    {
                        if (!EditorUtility.DisplayDialog("ViewModel Override", "Are you sure to override the exist ViewModel?", "Yes", "No"))
                        {
                            return;
                        }

                    }


                    int count = 0;
                    List<GameObject> goes = new List<GameObject>();
                    EditorSceneManager.GetActiveScene().GetRootGameObjects(goes);
                    Stack<GameObject> ds = new Stack<GameObject>();
                    goes.ForEach((x) => ds.Push(x));

                    while (ds.Count != 0)
                    {
                        EditorUtility.DisplayProgressBar("Build In Scene", "Stack Searching", (float)1 / ds.Count);
                        GameObject go = ds.Pop();
                        if (go.GetComponent<ScriptMark>() != null)
                        {
                            target = go;
                            BuildTarget();
                            count++;
                        }
                        else
                        {
                            foreach (Transform child in go.transform) ds.Push(child.gameObject);
                        }

                    }
                    EditorUtility.ClearProgressBar();
                    target = null;
                    Debug.Log($"Build In Scene Command . Total:{count}");
                }
                if (GUILayout.Button("Build All Prefabs"))
                {
                    if (!notVMOverride)
                    {
                        if (!EditorUtility.DisplayDialog("ViewModel Override", "Are you sure to override the exist ViewModel?", "Yes", "No"))
                        {
                            return;
                        }
                        
                    }


                    int count = 0;
                    List<GameObject> prefabs = new List<GameObject>();
                    var resourcesPath = Application.dataPath;
                    var absolutePaths = System.IO.Directory.GetFiles(resourcesPath, "*.prefab", System.IO.SearchOption.AllDirectories);
                    for (int i = 0; i < absolutePaths.Length; i++)
                    {
                        EditorUtility.DisplayProgressBar("Build All Prefabs Ref", "Prefabs finding", (float)i / absolutePaths.Length);

                        string path = "Assets" + absolutePaths[i].Remove(0, resourcesPath.Length);
                        path = path.Replace("\\", "/");
                        GameObject prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
                        if (prefab != null && GetMarks(prefab).Count > 0)
                            prefabs.Add(prefab);
                    }
                    for (int i = 0; i < prefabs.Count; i++)
                    {
                        EditorUtility.DisplayProgressBar("Build All Prefabs Ref", $"Build {prefabs[i].name}", (float)i / prefabs.Count);
                        target = prefabs[i];

                        BuildTarget();
                        count++;
                    }
                    EditorUtility.ClearProgressBar();
                    target = null;
                    Debug.Log($"Build All Prefabs . Total:{count}");
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
            }
        }


        void Build(string buildPath, GameObject buildTarget, string _namespace, string _class, bool increase, bool part)
        {

            try
            {
                //生成容器
                List<string> buildField = new List<string>();
                List<string> buildInit = new List<string>();

                List<ScriptMark> marks = GetMarks(buildTarget);
                int index = 0;
                foreach (ScriptMark mark in marks)
                {
                    //Mark数据准备
                    var properties = MaskHelper.SplitToMaskOptions(mark.buildProperty);
                    properties.RemoveAll((x) => x == string.Empty || x == null);

                    var com = mark.buildTarget;
                    string originGameObjectName = com.gameObject.name;
                    string pattern = "[^a-zA-Z0-9_\u4e00-\u9fa5]";
                    string gameObjectName = Regex.Replace(originGameObjectName, pattern, "_");



                    //组件属性构建
                    string componentFiled = increase ?
                    $"public {com.GetType().FullName} @{gameObjectName}_{com.GetType().Name}_{index} {{ get; private set; }} = default;" :
                    $"public {com.GetType().FullName} @{gameObjectName}_{com.GetType().Name} {{ get; private set; }} = default;";
                    buildField.Add(componentFiled);
                    string componentInit = string.Empty;





                    //逻辑生成分类
                    if (mark.buildTarget.gameObject == buildTarget)
                    {
                        //组件初始化
                        componentInit = increase ?
                            $"@{gameObjectName}_{com.GetType().Name}_{index} = GetComponent<{com.GetType().FullName}>();" :
                            $"@{gameObjectName}_{com.GetType().Name} = GetComponent<{com.GetType().FullName}>();";
                    }
                    else
                    {
                        //当前ScriptMark的Transform组件
                        Transform cur = com.transform;
                        string findPath = cur.gameObject.name;
                        //如果当前不是第一层子物体，则构建物体路径
                        while (cur.parent.gameObject != buildTarget.gameObject)
                        {
                            findPath = $"{cur.parent.gameObject.name}/" + findPath;
                            cur = cur.parent;
                        }


                        //组件初始化
                        componentInit = increase ?
                            $"@{gameObjectName}_{com.GetType().Name}_{index} = transform.Find(@\"{findPath}\").GetComponent<{com.GetType().FullName}>();" :
                            $"@{gameObjectName}_{com.GetType().Name} = transform.Find(@\"{findPath}\").GetComponent<{com.GetType().FullName}>();";
                    }
                    buildInit.Add(componentInit);
                    foreach (string property in properties)
                    {
                        var memberType = ReflectionHelper.GetFieldOrPropertyTypeByName(property, com);
                        string filed = increase ?
                            $"public {binderName}<{memberType.FullName}> @{gameObjectName}_{com.GetType().Name}_{index}_{property} {{get;set;}}=default;" :
                            $"public {binderName}<{memberType.FullName}> @{gameObjectName}_{com.GetType().Name}_{property} {{get;set;}}=default;";
                        string init = increase ?
                            $"@{gameObjectName}_{com.GetType().Name}_{index}_{property} = new {binderName}<{memberType.FullName}>(@{gameObjectName}_{com.GetType().Name}_{index},(y)=>(({com.GetType().FullName})y).{property},(y,x)=>(({com.GetType().FullName})y).{property}=x);" :
                            $"@{gameObjectName}_{com.GetType().Name}_{property} = new {binderName}<{memberType.FullName}>(@{gameObjectName}_{com.GetType().Name},(y)=>(({com.GetType().FullName})y).{property},(y,x)=>(({com.GetType().FullName})y).{property}=x);";

                        buildField.Add(filed);
                        buildInit.Add(init);
                    }
                    index++;
                }



                if (!Directory.Exists(Path.GetDirectoryName(buildPath))) Directory.CreateDirectory(Path.GetDirectoryName(buildPath));
                if (!File.Exists(buildPath)) File.Create(buildPath).Dispose();
                //Debug.Log("TryBuild :" + buildPath);

                using (StreamWriter writer = new StreamWriter(buildPath, false, Encoding.UTF8))
                {
                    StringBuilder fields_builder = new StringBuilder();
                    foreach (string str in buildField)
                    {
                        fields_builder.Append("        ");
                        fields_builder.AppendLine(str);
                    }
                    StringBuilder init_builder = new StringBuilder();
                    foreach (string str in buildInit)
                    {
                        init_builder.Append("            ");
                        init_builder.AppendLine(str);
                    }

                    string model =
    @"/*******************************************************
 * Code Generated By FFramework
 * DateTime : #DATETIME#
 * UVersion : #VERSION#
 *******************************************************/
using UnityEngine;
using FFramework.MVVM;

namespace #NAMESPACE#
{
    public #PART#class #CLASS# : #BASECLASS#
    {
#FIELDS#

        public void InitRefs()
        {
#INIT#
        }

        #NEWFUNC#
    }
    
}
";
                    model = model.Replace("#VERSION#", Application.unityVersion);
                    model = model.Replace("#DATETIME#", DateTime.Now.ToString());
                    model = model.Replace("#FIELDS#", fields_builder.ToString());
                    model = model.Replace("#INIT#", init_builder.ToString());
                    model = model.Replace("#NAMESPACE#", _namespace);
                    model = model.Replace("#CLASS#", _class);
                    model = model.Replace("#PART#", part ? "partial " : string.Empty);
                    model = model.Replace("#BASECLASS#", isUI?"FFramework.MVVM.UIPanel": "FFramework.MVVM.View");
                    model = model.Replace("#NEWFUNC#", awakeInit ?
    @"
        private void Awake()
        {
            InitRefs();
        }
" : string.Empty);

                    writer.Write(model);

                }

                if (generateViewModel)
                {
                    
                    string originGameObjectName = buildTarget.gameObject.name;
                    string pattern = "[^a-zA-Z0-9_\u4e00-\u9fa5]";
                    string gameObjectName = Regex.Replace(originGameObjectName, pattern, "_");

                    string scriptName =gameObjectName + "VM";
                    string absVMPath = Path.Combine(EditorHelper.ProjectPath, vmGenPath, scriptName + ".cs");
                    BuildVM(absVMPath, scriptName, "FFramework.MVVM.ViewModel", gameObjectName, _namespace);
                }

            }
            catch (Exception e)
            {
                Debug.LogError($"Build Ref Faulted!\nDetails = {e}");
                return;
            }
        }

        /// <summary>
        /// 生成ViewModel
        /// </summary>
        /// <param name="buildPath">生成路径</param>
        /// <param name="name">类名</param>
        /// <param name="vmName">vm类名</param>
        /// <param name="refName">View名</param>
        void BuildVM(string buildPath, string name, string vmName, string refName, string _namespace)
        {
            if (File.Exists(buildPath) && notVMOverride) return;
          

            string model =
@"
/*******************************************************
 * Code Generated By FFramework
 * DateTime : #DATETIME#
 * UVersion : #VERSION#
 *******************************************************/
using FFramework;
using FFramework.MVVM;

namespace #NAMESPACE#
{
    public class #CLASSNAME# : #VMNAME#<#REFNAME#>
    {
        //Select Text And Press Ctrl + K + U to uncomment.

        //public override async FTask OnLoad()
        //{
            //await FTask.CompletedTask;
        //}

        //public override async FTask OnUnload()
        //{
            //await FTask.CompletedTask;
        //}
    }
}

";
            model = model
                .Replace("#DATETIME#", DateTime.Now.ToString())
                .Replace("#VERSION#", Application.unityVersion.ToString())
                .Replace("#CLASSNAME#", name)
                .Replace("#VMNAME#", vmName)
                .Replace("#REFNAME#", _namespace + "." + refName)
                .Replace("#NAMESPACE#", _namespace);

            EditorHelper.NotExistCreate(buildPath);

            using (StreamWriter writer = new StreamWriter(buildPath))
            {
                writer.Write(model);
            }
        }

        List<ScriptMark> GetMarks(GameObject gameObject)
        {
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(gameObject.transform);
            List<ScriptMark> marks = new List<ScriptMark>();
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                foreach (Transform t in node.transform)
                {
                    queue.Enqueue(t);
                }
                var mks = node.GetComponents<ScriptMark>();
                for (int i = 0; i < mks.Length; i++)
                {
                    marks.Add(mks[i]);
                }
            }
            return marks;
        }



    }
}