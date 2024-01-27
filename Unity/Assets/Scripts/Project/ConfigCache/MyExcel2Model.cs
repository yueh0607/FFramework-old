using System;
using System.Collections;
using System.Collections.Generic;
  
namespace FFramework.RefCache
{

    public class MyExcel2Item
    {
        [FFramework.MVVM.Config.PrimaryKey]
        public System.Int32 id = default;
        public System.String description = default;
        public System.Single rate = default;
        public System.Type type = default;

    }

    public class MyExcel2Model : FFramework.MVVM.IModel
    {
        public System.Collections.Generic.List<MyExcel2Item> Data = new System.Collections.Generic.List<MyExcel2Item>()
        {
new MyExcel2Item(){id = 1,description = "神奇的魔法药水",rate = 2.5F,type = System.Type.GetType("int")}
,new MyExcel2Item(){id = 2,description = "雷霆",rate = 3F,type = System.Type.GetType("string")}
,new MyExcel2Item(){id = 3,description = "未知的代码力量",rate = 1F,type = System.Type.GetType("float")}

        };

        private System.Collections.Generic.Dictionary<System.Int32,MyExcel2Item> dataMap = null;
        public System.Collections.Generic.Dictionary<System.Int32,MyExcel2Item> DataMap 
        {
            get
            {
                if(dataMap==null)
                {
                    dataMap = new System.Collections.Generic.Dictionary<System.Int32,MyExcel2Item>();
                    foreach(var item in Data)
                    {
                        dataMap.Add(item.id,item);
                    }
                }
                return dataMap;
            }

        }

        public MyExcel2Model(){}
    }
}


