using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Zenject;
using static UIController;


public class UIController : MonoBehaviour, IUIWindowsController
{
    [SerializeField]
    UIDocument uiDocument;

    [Inject]
    SignalBus onStart, onRestart;

    Dictionary<UIWindowName, VisualElement> windowsDictionary;
    VisualElement root;

    Label scoreLabel;
    Label gameOverScoreLabel;
    Label fieldSizeLabel;
    SliderInt fieldSizeSlider;

    public int FieldSize
    {
        get => fieldSizeSlider.value;
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
    [Inject]
    void InitializeUIController()
    {
        StartCoroutine("InitializeCoroutine");
    }
    void CloseAllUIWindows()
    {
        foreach (KeyValuePair<UIWindowName, VisualElement> pair in windowsDictionary)
            pair.Value.style.display = DisplayStyle.None;
    }
    void AddNewWindowToDictionary(UIWindowName name)
    {
        if (windowsDictionary == null)
            windowsDictionary = new();

        if (root == null)
            root = uiDocument.rootVisualElement;

        if (windowsDictionary.ContainsKey(name))
            return;

        VisualElement buff = root.Q(name.ToString());

        windowsDictionary.Add(name, buff);
    }
    void DictionaryFill()
    {
        windowsDictionary = new Dictionary<UIWindowName, VisualElement>();

        AddNewWindowToDictionary(UIWindowName.GreetingsWindow);
        AddNewWindowToDictionary(UIWindowName.GameOverWindow);
        AddNewWindowToDictionary(UIWindowName.MainGameWindow);
    }
    IEnumerator InitializeCoroutine()
    {
        yield return new WaitUntil(() => 
            uiDocument.rootVisualElement != null);

        DictionaryFill();

        RegisterElements();

        OpenUIWindow(UIWindowName.GreetingsWindow);
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
    void IUIWindowsController.ChangeScore(int score)
    {
        scoreLabel.text = $"Score: {score}";
        gameOverScoreLabel.text = $"Score: {score}";
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
