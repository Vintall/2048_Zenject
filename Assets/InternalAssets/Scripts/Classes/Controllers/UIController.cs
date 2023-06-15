using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Zenject;
using static UIController;

public interface IUIWindowsController
{
    void OpenUIWindow(UIWindowName name);
    void ChangeScore(int score);
    public int FieldSize 
    { 
        get; 
    }
}
public class UIController : MonoBehaviour, IUIWindowsController
{
    Dictionary<UIWindowName, VisualElement> windowsDictionary;
    VisualElement root;

    [Inject]
    SignalBus onStart, onRestart;

    Label scoreLabel;
    Label gameOverScoreLabel;
    Label fieldSizeLabel;
    SliderInt fieldSizeSlider;

    public enum UIWindowName
    {
        GreetingsWindow,
        GameOverWindow,
        MainGameWindow
    }
    public int FieldSize
    {
        get => fieldSizeSlider.value;
    }
    //IGameController gameController;
    void AddNewWindowToDictionary(UIWindowName name)
    {
        if (windowsDictionary == null)
            windowsDictionary = new();

        if (root == null)
            root = GetComponent<UIDocument>().rootVisualElement;

        if (windowsDictionary.ContainsKey(name))
            return;

        windowsDictionary.Add(name, root.Q(name.ToString()));
    }
    void DictionaryFill()
    {
        AddNewWindowToDictionary(UIWindowName.GreetingsWindow);
        AddNewWindowToDictionary(UIWindowName.GameOverWindow);
        AddNewWindowToDictionary(UIWindowName.MainGameWindow);
    }
    void RegisterElements()
    {
        root.Q<Button>("StartButton").clicked += OnStartButtonClick;
        root.Q<Button>("RestartButton").clicked += OnRestartButtonClick;
        fieldSizeSlider = root.Q<SliderInt>("FieldSizeSlider");

        fieldSizeSlider.RegisterValueChangedCallback(OnSliderValueChanged);

        fieldSizeLabel = root.Q<Label>("FieldSizeLabel");
        scoreLabel = root.Q<Label>("ScoreLabel");
        gameOverScoreLabel = root.Q<Label>("GameOverScoreLabel");

    }

    public void OpenUIWindow(UIWindowName name)
    {
        if (windowsDictionary == null)
            return;

        if (!windowsDictionary.ContainsKey(name))
        {
            Debug.LogException(new("UI windows dictionary does not contain a specified key"));
            return;
        }
        CloseAllUIWindows();
        windowsDictionary[name].style.display = DisplayStyle.Flex;
    }
    private void CloseAllUIWindows()
    {
        foreach(KeyValuePair<UIWindowName, VisualElement> pair in windowsDictionary)
            pair.Value.style.display = DisplayStyle.None;
    }
    void IUIWindowsController.ChangeScore(int score)
    {
        scoreLabel.text = $"Score: {score}";
        gameOverScoreLabel.text = $"Score: {score}";
    }

    [Inject]
    void InitializeUIController()
    {
        DictionaryFill();

        RegisterElements();

        OpenUIWindow(UIWindowName.GreetingsWindow);
    }
    void OnSliderValueChanged(ChangeEvent<int> changeEvent)
    {
        fieldSizeLabel.text = $"Field size: {changeEvent.newValue}";
    }
    void OnRestartButtonClick()
    {
        onRestart.Fire(new RestartButtonSignal());
    }
    void OnStartButtonClick()
    {
        CloseAllUIWindows();

        onStart.Fire(new StartButtonSignal());
    }
}
