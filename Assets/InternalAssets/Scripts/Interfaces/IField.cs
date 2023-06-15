public interface IField
{
    public ITile[,] Tiles
    {
        get;
        set;
    }
    public int AxisLength
    {
        get;
        set;
    }
    public int FreeTilesCount
    {
        get;
    }
    
}
