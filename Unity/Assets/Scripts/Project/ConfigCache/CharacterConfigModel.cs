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
        public System.Single maxHp = default;
        public System.Single maxHungry = default;
        public System.Single recoveryHungry = default;
        public UnityEngine.Vector2 recRatio = default;
        public System.Single recTime = default;
        public System.Single initAtk = default;
        public System.Single defSpeed = default;
        public System.Int32 initPhysicDef = default;
        public System.Int32 initMagicDef = default;
        public System.Int32 trueDef = default;

    }

    public class CharacterConfigModel : FFramework.MVVM.IModel
    {
        public System.Collections.Generic.List<CharacterConfigItem> Data = new System.Collections.Generic.List<CharacterConfigItem>()
        {
new CharacterConfigItem(){rotationSensitivityY = 1F,rotationSensitivityX = 1F,maxPitchUp = 85F,maxPitchDown = 85F,moveSpeed = 1F,jumpForce = 1F,groundCheckBox = new UnityEngine.Vector3(1F,1F,1F),groundCheckOffset = new UnityEngine.Vector3(0F,1F,0F),maxHp = 100F,maxHungry = 100F,recoveryHungry = 60F,recRatio = new UnityEngine.Vector2(1F,1F),recTime = 2F,initAtk = 1F,defSpeed = 0.5F,initPhysicDef = 0,initMagicDef = 0,trueDef = 0}

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


