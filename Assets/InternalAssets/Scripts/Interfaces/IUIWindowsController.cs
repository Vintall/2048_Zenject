public interface IUIWindowsController
{
    void OpenUIWindow(UIWindowName name);
    void ChangeScore(int score);
    public int FieldSize
    {
        get;
    }
}