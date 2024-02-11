using System;
using System.Collections;
using System.Collections.Generic;
  
namespace FFramework.RefCache
{

    public class CharacterConfigItem
    {
        [FFramework.MVVM.Config.PrimaryKey]
        public System.Single rotationSensitivityY = default;
        public System.Single rotationSensitivityX = default;
        public System.Single maxPitchUp = default;
        public System.Single maxPitchDown = default;
        public System.Single moveSpeed = default;
        public System.Single jumpForce = default;
        public UnityEngine.Vector3 groundCheckBox = default;
        public UnityEngine.Vector3 groundCheckOffset = default;

    }

    public class CharacterConfigModel : FFramework.MVVM.IModel
    {
        public System.Collections.Generic.List<CharacterConfigItem> Data = new System.Collections.Generic.List<CharacterConfigItem>()
        {
new CharacterConfigItem(){rotationSensitivityY = 1F,rotationSensitivityX = 1F,maxPitchUp = 85F,maxPitchDown = 85F,moveSpeed = 1F,jumpForce = 1F,groundCheckBox = new UnityEngine.Vector3(1F,1F,1F),groundCheckOffset = new UnityEngine.Vector3(0F,1F,0F)}

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
                        dataMap.Add(item.rotationSensitivityY,item);
                    }
                }
                return dataMap;
            }

        }

        public CharacterConfigModel(){}
    }
}


