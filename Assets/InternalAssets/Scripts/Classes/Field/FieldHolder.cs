using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldHolder : IField
{
    ITile[,] tiles;
    int axisLength;
    public ITile[,] Tiles
    {
        get => tiles;
        set => tiles = value;
    }
    public int AxisLength
    {
        get => axisLength;
        set => axisLength = value;
    }

    public int FreeTilesCount
    {
        get
        {
            if (tiles == null)
                return 0;

            int count = 0;
            for (int i = 0; i < axisLength; ++i)
                for (int j = 0; j < axisLength; ++j)
                    if (tiles[i, j].TileState == TileState.Free)
                        ++count;

            return count;
        }
    }
}
