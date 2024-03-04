using FFramework;
using FFramework.MVVM;
using FFramework.RefCache;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : IModel
{
    //角色位置
    public BindableProperty<Vector3Int> Position = new BindableProperty<Vector3Int>(new Vector3Int(0, 0, 0));

    //生命值
    public BindableProperty<float> Hp = new BindableProperty<float>(SingletonProperty<CharacterConfigModel>.Instance.Data[0].maxHp);
    //最大生命值
    public BindableProperty<float> MaxHp = new BindableProperty<float>(SingletonProperty<CharacterConfigModel>.Instance.Data[0].maxHp);
    //饥饿度
    public BindableProperty<float> Hungry = new BindableProperty<float>(SingletonProperty<CharacterConfigModel>.Instance.Data[0].maxHungry);
    //最大饥饿度
    public BindableProperty<float> MaxHungry = new BindableProperty<float>(SingletonProperty<CharacterConfigModel>.Instance.Data[0].maxHungry);

    //真实抗性
    public BindableProperty<int> TrueDef = new BindableProperty<int>(SingletonProperty<CharacterConfigModel>.Instance.Data[0].trueDef);
    //物理抗性
    public BindableProperty<int> PhysicDef = new BindableProperty<int>(SingletonProperty<CharacterConfigModel>.Instance.Data[0].initPhysicDef);
    //法术抗性
    public BindableProperty<int> MagicDef = new BindableProperty<int>(SingletonProperty<CharacterConfigModel>.Instance.Data[0].initMagicDef);

    //额外的物理攻击力
    public BindableProperty<int> AtkExtraPhysic = new BindableProperty<int>(0);
    //额外的法术攻击力
    public BindableProperty<int> AtkExtraMagic = new BindableProperty<int>(0);

    //角色是否存活
    public BindableProperty<bool> IsLive = new BindableProperty<bool>(true);

    public PlayerModel()
    {
        //如果血量小于等于0，角色死亡，同步更新存活值
        Hp.OnPropertyChanged += (oldValue, newValue) =>
        {
            if (newValue <= 0)
            {
                IsLive.Value = false;
            }
        };
        //如果角色存活状态改变，启动生命恢复
        IsLive.OnPropertyChanged += (oldValue, newValue) =>
        {
            if (newValue)
            {
                LoopRecHp().Forget();
            }
        };

        //区块更新事件
        Position.OnPropertyChanged += (oldValue, newValue) =>
        {
            //计算区块坐标
            var tuple = newValue.CalChunkPos();
            //方块相对于区块原点的坐标
            Vector3Int blockPosInChunk = tuple.Item2;
            //区块原点的坐标
            Vector3Int chunkPos = tuple.Item1;
            //Debug.Log($"区块坐标:{chunkPos}");

            var mapMpdel = MV.GetModel<MapModel>();
            for (int xOffset = -MapModel.ChunkLoadCount; xOffset < MapModel.ChunkLoadCount; xOffset++)
                for (int yOffset = -MapModel.ChunkLoadCount; yOffset < MapModel.ChunkLoadCount; yOffset++)
                    for (int zOffset = -MapModel.ChunkLoadCount; zOffset < MapModel.ChunkLoadCount; zOffset++)
                    {   //试着加载周围的N个区块
                        var chunk = mapMpdel.GetChunk(chunkPos +
                            new Vector3Int(MapModel.ChunkSize.x * xOffset, MapModel.ChunkSize.y * yOffset, MapModel.ChunkSize.z * zOffset));

                        if(chunk!=null)
                        {
                            int a = 0;
                        }
                        chunk?.CreateChunk().Forget();


                    }




        };
    }

    /// <summary>
    /// 间歇性恢复生命值
    /// </summary>
    /// <returns></returns>
    private async FTask LoopRecHp()
    {
        var config = MV.GetModel<CharacterConfigModel>().Data[0];
        //如果存活
        while (IsLive.Value)
        {
            //Debug.Log("计时");
            //间隔一定的恢复时间
            await FTask.Delay(config.recTime);
            //Debug.Log("恢复生命值");
            //检查是否饥饿度足够  生命是否满了
            if (Hungry.Value < config.recoveryHungry || Hp.Value >= MaxHp.Value) continue;
            //饥饿度转换为生命值
            Hungry.Value -= config.recRatio.x;
            Hp.Value = Math.Clamp(Hp.Value + config.recRatio.y, 0, MaxHp.Value);
        }
    }
}
