using FFramework;
using FFramework.AI.FSM;
using FFramework.MVVM;
using FFramework.MVVM.RefCache;
using FFramework.MVVM.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : State
{

    MenuCameraVM mvm;
    bool eveInit = false;
    public override async FTask OnEnter()
    {
        if (!eveInit)
        {
            //加载后直接，永久存在
            await MV.Load<EventSystemVM, EventSystem>();
            eveInit = true;

            //读取存档
            await MV.ReadSaves();
        }
        
        

        //打开主页面板
        await UI.Show<MenuPanelVM, MenuPanel>();
        //创建相机
        mvm = await MV.Load<MenuCameraVM,MenuCamera>();
        await FTask.CompletedTask;
    }

    public override async FTask OnExit()
    {
        //销毁相机
        await MV.Unload(mvm);
        //隐藏主页面板
        await UI.Hide<MenuPanelVM, MenuPanel>();
        
        await FTask.CompletedTask;
    }

    public override void OnStay(float deltaTime)
    {
       
    }
}
