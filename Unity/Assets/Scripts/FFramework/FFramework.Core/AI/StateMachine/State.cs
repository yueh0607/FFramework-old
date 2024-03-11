using FFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FFramework.AI.FSM
{
    public abstract class State
    {
        public abstract FTask OnEnter();


        public abstract FTask OnExit();


        public abstract void OnStay(float deltaTime);


    }
}