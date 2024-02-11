using System;
using System.Collections;
using System.Collections.Generic;
  
namespace FFramework.RefCache
{

    public class CharacterConfigItem
    {
        [FFramework.MVVM.Config.PrimaryKey]
        public System.Single rotationSensitivity = default;
        public System.Single maxPitchUp = default;
        public System.Single maxPitchDown = default;

    }

    public class CharacterConfigModel : FFramework.MVVM.IModel
    {
        public System.Collections.Generic.List<CharacterConfigItem> Data = new System.Collections.Generic.List<CharacterConfigItem>()
        {
new CharacterConfigItem(){rotationSensitivity = 0.1F,maxPitchUp = 45F,maxPitchDown = 60F}

        };

        private System.Collections.Generic.Dictionary<System.Single,CharacterConfigItem> dataMap = null;
        public System.Collections.Generic.Dictionary<System.Single,CharacterConfigItem> DataMap 
        {
            get
            {
                if(dataMap==null)
                {
                    dataMap = new System.Collections.Generic.Dictionary<System.Single,CharacterConfigItem>();
                    foreach(var item in Data)
                    {
                        dataMap.Add(item.rotationSensitivity,item);
                    }
                }
                return dataMap;
            }

        }

        public CharacterConfigModel(){}
    }
}


