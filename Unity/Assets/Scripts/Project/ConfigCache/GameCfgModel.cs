using System;
using System.Collections;
using System.Collections.Generic;
  
namespace FFramework.RefCache
{

    public class GameCfgItem
    {
        [FFramework.MVVM.Config.PrimaryKey]
        public System.Single planeMoveSpeed = default;
        public System.Single rotSpeed = default;
        public System.Single bulletDuration = default;
        public System.Single enemyMoveSpeed = default;
        public System.Single enemyRotSpeed = default;
        public System.Single enemyDensity = default;
        public System.Single missileMoveMaxSpeed = default;
        public System.Single missileAcc = default;
        public System.Single missileRotSpeed = default;

    }

    public class GameCfgModel : FFramework.MVVM.IModel
    {
        public System.Collections.Generic.List<GameCfgItem> Data = new System.Collections.Generic.List<GameCfgItem>()
        {
new GameCfgItem(){planeMoveSpeed = 1F,rotSpeed = 1F,bulletDuration = 5F,enemyMoveSpeed = 1F,enemyRotSpeed = 1F,enemyDensity = 0.1F,missileMoveMaxSpeed = 1F,missileAcc = 1F,missileRotSpeed = 1F}

        };

        private System.Collections.Generic.Dictionary<System.Single,GameCfgItem> dataMap = null;
        public System.Collections.Generic.Dictionary<System.Single,GameCfgItem> DataMap 
        {
            get
            {
                if(dataMap==null)
                {
                    dataMap = new System.Collections.Generic.Dictionary<System.Single,GameCfgItem>();
                    foreach(var item in Data)
                    {
                        dataMap.Add(item.planeMoveSpeed,item);
                    }
                }
                return dataMap;
            }

        }

        public GameCfgModel(){}
    }
}


