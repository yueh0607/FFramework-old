using System;
using System.Collections;
using System.Collections.Generic;
  
namespace FFramework.RefCache
{

    public class BasicBlockPropertiesItem
    {
        [FFramework.MVVM.Config.PrimaryKey]
        public System.Int32 id = default;
        public System.String name = default;
        public System.Boolean isTransparent = default;
        public System.Int32 hardness = default;
        public System.Int32 minToolLevel = default;
        public System.String prefabPath = default;
        public System.String matPath = default;
        public System.String brokenSound = default;
        public System.String placeSound = default;
        public System.String exploreSound = default;
        public System.Int32[] categories = default;

    }

    public class BasicBlockPropertiesModel : FFramework.MVVM.IModel
    {
        public System.Collections.Generic.List<BasicBlockPropertiesItem> Data = new System.Collections.Generic.List<BasicBlockPropertiesItem>()
        {
new BasicBlockPropertiesItem(){id = 0,name = "石头",isTransparent = false,hardness = 30,minToolLevel = 1,prefabPath = "Block",matPath = "Stone",brokenSound = "",placeSound = "",exploreSound = "",categories = new System.Int32[1]{0,}}
,new BasicBlockPropertiesItem(){id = 1,name = "泥土",isTransparent = false,hardness = 10,minToolLevel = 0,prefabPath = "Block",matPath = "Dirt",brokenSound = "",placeSound = "",exploreSound = "",categories = new System.Int32[1]{0,}}

        };

        private System.Collections.Generic.Dictionary<System.Int32,BasicBlockPropertiesItem> dataMap = null;
        public System.Collections.Generic.Dictionary<System.Int32,BasicBlockPropertiesItem> DataMap 
        {
            get
            {
                if(dataMap==null)
                {
                    dataMap = new System.Collections.Generic.Dictionary<System.Int32,BasicBlockPropertiesItem>();
                    foreach(var item in Data)
                    {
                        dataMap.Add(item.id,item);
                    }
                }
                return dataMap;
            }

        }

        public BasicBlockPropertiesModel(){}
    }
}


