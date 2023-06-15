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

    [Inject(Id = "UIController")]
    IUIWindowsController uiController;

    [Inject(Id = "FieldSpawner")]
    IFieldSpawner fieldSpawner;

    [Inject] 
    IField field;

    [Inject]
    IFieldController fieldController;

    int score = 0;

    void StartGame()
    {
        gameState = GameState.Running;
        uiController.OpenUIWindow(UIController.UIWindowName.MainGameWindow);

        fieldSpawner.SpawnField(field, uiController.FieldSize);

        fieldController.SpawnTiles(field);
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

        int[,] scoreBuffer = new int[field.AxisLength, field.AxisLength];

        for (int i = 0; i < field.AxisLength; ++i)
            for (int j = 0; j < field.AxisLength; ++j)
                scoreBuffer[i, j] = field.Tiles[i, j].TileScore;


        int scoreAddition = fieldController.InputHandle(field, swipeDirection.swipeDirection);
        score += scoreAddition;

        uiController.ChangeScore(score);
        

        bool result = fieldController.CheckField(field);

        if(!result)
        {
            Debug.Log("GameOverEvent");
            uiController.OpenUIWindow(UIController.UIWindowName.GameOverWindow);

            gameState = GameState.GameOver;

            return;
        }
        bool isSpawnAllowed = false;

        for (int i = 0; i < field.AxisLength; ++i)
            for (int j = 0; j < field.AxisLength; ++j)
                if (field.Tiles[i, j].TileScore != scoreBuffer[i, j])
                {
                    isSpawnAllowed = true;
                    break;
                }

        if (isSpawnAllowed)
            fieldController.SpawnTiles(field);
    }

}
