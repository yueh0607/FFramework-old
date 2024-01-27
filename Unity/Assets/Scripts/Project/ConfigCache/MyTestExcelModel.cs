using System;
using System.Collections;
using System.Collections.Generic;
  
namespace FFramework.RefCache
{

    public class MyTestExcelItem
    {
        [FFramework.MVVM.Config.PrimaryKey]
        public System.Int32 id = default;
        public System.String name = default;
        public System.Single age = default;
        public UnityEngine.Color clothColor = default;
        public UnityEngine.Vector2 pos = default;
        public System.String[] tags = default;
        public System.Int32[] abi = default;
        public MyExcel2Item pros = default;

    }

    public class MyTestExcelModel : FFramework.MVVM.IModel
    {
        public System.Collections.Generic.List<MyTestExcelItem> Data = new System.Collections.Generic.List<MyTestExcelItem>()
        {
new MyTestExcelItem(){id = 1,name = "hh",age = 2F,clothColor = new UnityEngine.Color(1F, 1F, 1F, 0.9960784F),pos = new UnityEngine.Vector2(10F,21F),tags = new System.String[2]{"gentle","beautiful"},abi = new System.Int32[2]{12,32,},pros = FFramework.SingletonProperty<MyExcel2Model>.Instance.DataMap[3]}
,new MyTestExcelItem(){id = 2,name = "hs",age = 5.02F,clothColor = new UnityEngine.Color(1F, 1F, 1F, 0.9921569F),pos = new UnityEngine.Vector2(10F,22F),tags = new System.String[2]{"gentle","beautiful"},abi = new System.Int32[2]{0,12,},pros = FFramework.SingletonProperty<MyExcel2Model>.Instance.DataMap[1]}
,new MyTestExcelItem(){id = 3,name = "hk",age = 4F,clothColor = new UnityEngine.Color(1F, 1F, 1F, 0.9843137F),pos = new UnityEngine.Vector2(10F,23F),tags = new System.String[1]{"gentle"},abi = new System.Int32[1]{64,},pros = FFramework.SingletonProperty<MyExcel2Model>.Instance.DataMap[2]}

        };

        private System.Collections.Generic.Dictionary<System.Int32,MyTestExcelItem> dataMap = null;
        public System.Collections.Generic.Dictionary<System.Int32,MyTestExcelItem> DataMap 
        {
            get
            {
                if(dataMap==null)
                {
                    dataMap = new System.Collections.Generic.Dictionary<System.Int32,MyTestExcelItem>();
                    foreach(var item in Data)
                    {
                        dataMap.Add(item.id,item);
                    }
                }
                return dataMap;
            }

        }

        public MyTestExcelModel(){}
    }
}


