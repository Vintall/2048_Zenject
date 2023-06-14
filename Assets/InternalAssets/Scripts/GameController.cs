using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public interface IGameController : ISwipeControlReceiver
{
    void OnRestartButtonClick();
    void OnStartButtonClick();
}
public interface ISwipeControlReceiver
{
    void ReceivePlayerSwipeInput(PlayerControlSwapSignal swipeDirection);
}
public class GameController : IGameController
{
    [Inject(Id = "Field")] 
    IField field;

    enum GameState
    {
        BeforeStart,
        Running,
        GameOver
    }

    GameState gameState = GameState.BeforeStart;
    public GameController()
    {
        
    }
    void StartGame()
    {
        Debug.Log("On game start");
        gameState = GameState.Running;

        field.SpawnTiles();
    }

    void IGameController.OnRestartButtonClick()
    {
        Debug.Log("OnRestartButtonClick");
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    void IGameController.OnStartButtonClick()
    {
        StartGame();
    }
    void ISwipeControlReceiver.ReceivePlayerSwipeInput(PlayerControlSwapSignal swipeDirection)
    {
        if (gameState != GameState.Running)
            return;

        field.InputHandle(swipeDirection.swipeDirection);
        //Debug.Log($"Swipe receive: {swipeDirection.swipeDirection}");
    }

}
