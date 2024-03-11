using FFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework.AI.FSM
{
    public class StateMachine : IDisposable, IUpdate
    {


        /// <summary>
        /// 当前状态
        /// </summary>
        public State Current { get; private set; }


        public StateMachine()
        {
            this.EnableUpdate();
        }
        public void Dispose()
        {
            this.DisableUpdate();
        }


        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="state"></param>
        public void Switch(State state)
        {
            Current?.OnExit();
            Current = state;
            Current?.OnEnter();
        }

        void IUpdate.Update(float deltaTime)
        {
            Current?.OnStay(deltaTime);
        }
    }
}