
/*******************************************************
 * Code Generated By FFramework
 * DateTime : 2024/3/7 23:08:53
 * UVersion : 2021.3.29f1
 *******************************************************/
using FFramework;
using FFramework.MVVM;
using System;

namespace FFramework.MVVM.RefCache
{
    public class GameMainPanelVM : FFramework.MVVM.UIPanel<FFramework.MVVM.RefCache.GameMainPanel>
    {
        //Select Text And Press Ctrl + K + U to uncomment.

        public override async FTask OnLoad()
        {
            TView.InitRefs();

            var model = MV.GetModel<PlaneModel>();

            //同步位置变化到UI
            model.PlanePosition.OnPropertyChanged += (oldValue, newValue) =>
            {
                TView.PlanePositionText_Text.text = $"({Math.Round(newValue.x)},{Math.Round(newValue.z)})";
            };
           

            //击杀数同步
            model.killCount.OnPropertyChanged += (oldValue, newValue) =>
            {
                TView.KillCountText_Text.text = $"击杀: {newValue}";
            };

            await FTask.CompletedTask;
        }

        public override async FTask OnUnload()
        {
            await FTask.CompletedTask;
        }
    }
}
