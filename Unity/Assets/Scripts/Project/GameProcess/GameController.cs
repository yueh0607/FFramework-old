using FFramework.AI.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController 
{
    public StateMachine Machine { get; private set; }
    public MenuState MenuState { get; private set; } 
    public GameState GameState { get; private set; }

    public  GameController()
    {
        MenuState = new MenuState();
        GameState = new GameState();
        Machine = new StateMachine();
        Machine.Switch(MenuState);
    }

    public void SwitchMenu()
    {
        Machine.Switch(MenuState);
    }

    public void SwitchGame()
    {
        Machine.Switch(GameState);
    }
    
}


