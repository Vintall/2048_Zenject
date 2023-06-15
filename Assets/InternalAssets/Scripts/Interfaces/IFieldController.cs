public interface IFieldController
{
    public void SpawnTiles(IField field);
    public bool CheckField(IField field);
    public int InputHandle(IField field, SwipeDirection swipeDirection);
}
