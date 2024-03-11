using System;
using System.Collections;
using System.Collections.Generic;
  
namespace FFramework.RefCache
{

    public class GameCfgItem
    {
        [FFramework.MVVM.Config.PrimaryKey]
        public System.Single planeMoveSpeed = default;
        public System.Single planeStableSpeed = default;
        public System.Single rotSpeed = default;
        public System.Single bulletDuration = default;
        public System.Single enemyMoveSpeed = default;
        public System.Single enemyRotSpeed = default;
        public System.Single enemyCount = default;
        public System.Single missileMoveMaxSpeed = default;
        public System.Single missileRotSpeed = default;

    }

    public class GameCfgModel : FFramework.MVVM.IModel
    {
        public System.Collections.Generic.List<GameCfgItem> Data = new System.Collections.Generic.List<GameCfgItem>()
        {
new GameCfgItem(){planeMoveSpeed = 50F,planeStableSpeed = 10F,rotSpeed = 60F,bulletDuration = 5F,enemyMoveSpeed = 50F,enemyRotSpeed = 0.5F,enemyCount = 8F,missileMoveMaxSpeed = 60F,missileRotSpeed = 0.6F}

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


