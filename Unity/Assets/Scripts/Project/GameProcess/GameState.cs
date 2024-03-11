using FFramework;
using FFramework.AI.FSM;
using FFramework.MVVM;
using FFramework.MVVM.RefCache;
using FFramework.MVVM.UI;
using FFramework.RefCache;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : State
{

    FTaskToken token;
    public PlaneVM Player => player;
    PlaneVM player;

    List<IViewModel> unloadList = new List<IViewModel>();
    public override async FTask OnEnter()
    {
        //光源
        var sun = await MV.Load<SunVM, Sun>();
       
        //加载角色
        player = await MV.Load<PlaneVM, FFramework.MVVM.RefCache.Plane>();
        //加载相机
        var camera = await MV.Load<FollowCameraVM, FollowCamera>();
        camera.SetFollow(player.TView.transform);
        
        //加载UI面板
        var gamePanel = await UI.GetPanel<GameMainPanelVM, GameMainPanel>();

        unloadList.Add(sun);
        unloadList.Add(camera);
        unloadList.Add(player);
        //生成怪物
        token = GenerateEnemy().Token;


        await FTask.CompletedTask;
    }

    public override async FTask OnExit()
    {

        //停止生成任务
        token.Cancel();
        token = null;

        await UI.Hide<GameMainPanelVM, GameMainPanel>();


        for (int i = 0; i < unloadList.Count; i++)
        {
            await MV.Unload(unloadList[i]);
        }
        unloadList.Clear();

        //卸载角色
        await MV.Unload(player);
        player = null;

        //卸载导弹
        for (int i = 0; i < missiles.Count; i++)
        {
            await MV.Unload<MissileVM>(missiles[i]);
        }
        missiles.Clear();


        //卸载怪物
        for (int i  =0; i < enemies.Count; i++)
        {
            await MV.UnloadToPool<EnemyVM,EnemyVM.EnemyPoolable>(enemies[i]);
        }
        enemies.Clear();
       




        //最大击杀数
        var planeModel = MV.GetModel<PlaneModel>();
        if(planeModel.killCount.Value>planeModel.maxKillCount.Value)
        {
            planeModel.maxKillCount.Value = planeModel.killCount.Value;
        }
        planeModel.killCount.Value = 0;

        //存档
        await MV.SaveModels();

        await UI.Hide<GameOverPanelVM, GameOverPanel>();
        await FTask.CompletedTask;
    }


    /// <summary>
    /// 敌人
    /// </summary>
    public List<EnemyVM> enemies { get; private set; } = new List<EnemyVM>();

    /// <summary>
    /// 导弹
    /// </summary>

    public List<MissileVM> missiles { get; private set; } = new List<MissileVM>();
    async FTask GenerateEnemy()
    {
        var model = MV.GetModel<GameCfgModel>().Data[0];
        while(enemies.Count<model.enemyCount)
        {
            Debug.Log("3s后生成敌人");
            await FTask.Delay(3);
            var enemy = await MV.LoadFromPool<EnemyVM, Enemy,EnemyVM.EnemyPoolable>();
            enemies.Add(enemy);
            Debug.Log("生成敌人");
    
        }
    }



    /// <summary>
    /// 获取一个随机的敌人
    /// </summary>
    /// <returns></returns>
    public EnemyVM GetRandomEnemy()
    {
        if (enemies.Count == 0)
            return null;
        return enemies[Random.Range(0, enemies.Count)];
    }
    


    public override void OnStay(float deltaTime)
    {



    }
}
