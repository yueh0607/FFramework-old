using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildinFileManifest : ScriptableObject
{
    [Serializable]
    public class Element
    {
        public string PackageName;
        public string FileName;
        public string FileCRC32;
    }

    public List<Element> BuildinFiles = new List<Element>();
}
