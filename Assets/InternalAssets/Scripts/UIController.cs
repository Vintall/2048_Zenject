using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Zenject;

public class UIController : MonoBehaviour
{
    IGameController gameController;

    VisualElement greetPanel;
    VisualElement gameOverPanel;

    [Inject]
    void InitializeUIController(IGameController gameController)
    {
        this.gameController = gameController;

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        greetPanel = root.Q("GreetingsWindow");
        gameOverPanel = root.Q("GameOverWindow");

        //gameOver.style.display = DisplayStyle.None;

        Button startButton = root.Q<Button>("StartButton");
        startButton.clicked += OnStartButtonClick;

        Button restartButton = root.Q<Button>("RestartButton");
        restartButton.clicked += OnRestartButtonClick;

        
    }
    void OnRestartButtonClick() => gameController.OnRestartButtonClick();
    void OnStartButtonClick() => gameController.OnStartButtonClick();
}
