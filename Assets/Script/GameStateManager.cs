using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { MainMenu, Gameplay, Pause }

public class GameStateManager : SimpleSingleton<GameStateManager>
{
    private GameState gameState;

    private void Start()
    {
        ChangeState(GameState.Gameplay);
    }

    public void ChangeState(GameState gameState)
    {
        this.gameState = gameState;
    }

    public bool IsState(GameState gameState)
    {
        return this.gameState == gameState;
    }
}
