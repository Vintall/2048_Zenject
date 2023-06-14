using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameController : IGameController
{
    enum GameState
    {
        BeforeStart,
        Running,
        GameOver
    }
    GameState gameState = GameState.BeforeStart;

    [Inject(Id = "Field")] 
    IField field;

    [Inject(Id = "UIController")]
    IUIWindowsController uiController;

    int score = 0;

    void StartGame()
    {
        gameState = GameState.Running;
        uiController.OpenUIWindow(UIController.UIWindowName.MainGameWindow);

        field.SpawnTiles();
    }

    void IButtonsCallbackReceiver.OnRestartButtonClick()
    {
        Debug.Log("OnRestartButtonClick");
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    void IButtonsCallbackReceiver.OnStartButtonClick()
    {
        StartGame();
    }
    void ISwipeControlReceiver.ReceivePlayerSwipeInput(PlayerControlSwapSignal swipeDirection)
    {
        if (gameState != GameState.Running)
            return;

        int scoreAddition = field.InputHandle(swipeDirection.swipeDirection);
        score += scoreAddition;

        uiController.ChangeScore(score);
        

        bool result = field.CheckField();

        if(!result)
        {
            Debug.Log("GameOverEvent");
            uiController.OpenUIWindow(UIController.UIWindowName.GameOverWindow);

            gameState = GameState.GameOver;

            return;
        }

        field.SpawnTiles();
    }

}
