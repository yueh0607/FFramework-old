using System;
using System.Collections;
using System.Collections.Generic;
  
namespace FFramework.RefCache
{

    public class BlockTagsItem
    {
        [FFramework.MVVM.Config.PrimaryKey]
        public System.Int32 id = default;
        public System.String name = default;

    }

    public class BlockTagsModel : FFramework.MVVM.IModel
    {
        public System.Collections.Generic.List<BlockTagsItem> Data = new System.Collections.Generic.List<BlockTagsItem>()
        {
new BlockTagsItem(){id = 0,name = "默认特性"}
,new BlockTagsItem(){id = 1,name = "土沙挖掘加速"}
,new BlockTagsItem(){id = 2,name = "矿石挖掘加速"}
,new BlockTagsItem(){id = 3,name = "木植挖掘加速"}
,new BlockTagsItem(){id = 4,name = "易碎挖掘"}

        };

        private System.Collections.Generic.Dictionary<System.Int32,BlockTagsItem> dataMap = null;
        public System.Collections.Generic.Dictionary<System.Int32,BlockTagsItem> DataMap 
        {
            get
            {
                if(dataMap==null)
                {
                    dataMap = new System.Collections.Generic.Dictionary<System.Int32,BlockTagsItem>();
                    foreach(var item in Data)
                    {
                        dataMap.Add(item.id,item);
                    }
                }
                return dataMap;
            }

        }

        public BlockTagsModel(){}
    }
}


