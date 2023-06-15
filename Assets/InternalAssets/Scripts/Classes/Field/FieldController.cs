using System.Collections.Generic;
using UnityEngine;


public class FieldController : IFieldController
{
    static int MaxTilesSpawn = 3;
    static int MinTilesSpawn = 2;

    void IFieldController.SpawnTiles(IField fieldHolder)
    {
        if (fieldHolder.FreeTilesCount == 0)
            return;

        int tilesToSpawn = Random.Range(MinTilesSpawn, 1 + Mathf.Min(fieldHolder.FreeTilesCount, MaxTilesSpawn));

        if (tilesToSpawn > fieldHolder.FreeTilesCount)
            tilesToSpawn = fieldHolder.FreeTilesCount;

        List<(int,int)> freeTileIndexes = new List<(int, int)>();

        for (int i = 0; i < fieldHolder.AxisLength; ++i)
            for (int j = 0; j < fieldHolder.AxisLength; ++j)
                if (fieldHolder.Tiles[i, j].TileState == TileState.Free)
                    freeTileIndexes.Add((i, j));

        (int, int)[] tilesToSpawnIndexes = new (int, int)[tilesToSpawn];
        for (int i = 0; i < tilesToSpawn; ++i)
        {
            int index = Random.Range(0, freeTileIndexes.Count);
            tilesToSpawnIndexes[i] = freeTileIndexes[index];
            freeTileIndexes.RemoveAt(index);

            fieldHolder.Tiles[tilesToSpawnIndexes[i].Item1, tilesToSpawnIndexes[i].Item2].TileScore = 2;
            fieldHolder.Tiles[tilesToSpawnIndexes[i].Item1, tilesToSpawnIndexes[i].Item2].TileState = TileState.Occupied;
        }

    }
    bool IFieldController.CheckField(IField fieldHolder)
    {
        bool isProceedPossible = false;
        for (int i = 0; i < fieldHolder.AxisLength; ++i)
            for (int j = 0; j < fieldHolder.AxisLength; ++j)
            {
                if (i < fieldHolder.AxisLength - 1)
                    if (fieldHolder.Tiles[i, j].TileScore == fieldHolder.Tiles[i + 1, j].TileScore)
                    {
                        isProceedPossible = true;
                        break;
                    }
                if (j < fieldHolder.AxisLength - 1)
                    if (fieldHolder.Tiles[i, j].TileScore == fieldHolder.Tiles[i, j + 1].TileScore)
                    {
                        isProceedPossible = true;
                        break;
                    }
                if(fieldHolder.Tiles[i, j].TileState == TileState.Free)
                {
                    isProceedPossible = true;
                    break;
                }
            }
        return isProceedPossible;
    }
    int IFieldController.InputHandle(IField fieldHolder, SwipeDirection swipeDirection)
    { // Definitely require refactoring. But it's enough for some functional state
        int scoreResult = 0;
        List<int> fullTileIndex;

        switch(swipeDirection)
        {
            case SwipeDirection.Left:
                LeftSwipe();
                break;
            case SwipeDirection.Up:
                UpSwipe();
                break;
            case SwipeDirection.Right:
                RightSwipe();
                break;
            case SwipeDirection.Down:
                DownSwipe();
                break;
        }
        void LeftSwipe()
        {
            for (int i = 0; i < fieldHolder.AxisLength; ++i)
            {
                void Aligning()
                {
                    for (int j = 0; j < fieldHolder.AxisLength; ++j)
                        if (fieldHolder.Tiles[i, j].TileState == TileState.Occupied)
                            fullTileIndex.Add(j);

                    for (int j = 0; j < fullTileIndex.Count; ++j)
                    {
                        int scoreBuff = fieldHolder.Tiles[i, fullTileIndex[j]].TileScore;

                        fieldHolder.Tiles[i, fullTileIndex[j]].TileState = TileState.Free;
                        fieldHolder.Tiles[i, fullTileIndex[j]].TileScore = 0;

                        fieldHolder.Tiles[i, j].TileScore = scoreBuff;
                        fieldHolder.Tiles[i, j].TileState = TileState.Occupied;
                    }
                }
                fullTileIndex = new List<int>(fieldHolder.AxisLength);
                Aligning();



                // Merging
                for (int j = 0; j < fullTileIndex.Count - 1; ++j)
                {
                    if (fieldHolder.Tiles[i, j].TileScore != fieldHolder.Tiles[i, j + 1].TileScore)
                        continue;

                    if (fieldHolder.Tiles[i, j].TileState == TileState.Free)
                        continue;

                    ++scoreResult;

                    fieldHolder.Tiles[i, j].TileScore *= 2;
                    fieldHolder.Tiles[i, j + 1].TileScore = 0;
                    fieldHolder.Tiles[i, j + 1].TileState = TileState.Free;
                }

                fullTileIndex = new List<int>(fieldHolder.AxisLength);
                Aligning();
            }
        }
        void UpSwipe()
        {
            for (int i = 0; i < fieldHolder.AxisLength; ++i) // Columns
            {
                void Aligning()
                {
                    for (int j = 0; j < fieldHolder.AxisLength; ++j) // Rows
                        if (fieldHolder.Tiles[j, i].TileState == TileState.Occupied)
                            fullTileIndex.Add(j);

                    for (int j = 0; j < fullTileIndex.Count; ++j)
                    {
                        int scoreBuff = fieldHolder.Tiles[fullTileIndex[j], i].TileScore;

                        fieldHolder.Tiles[fullTileIndex[j], i].TileState = TileState.Free;
                        fieldHolder.Tiles[fullTileIndex[j], i].TileScore = 0;

                        fieldHolder.Tiles[j, i].TileScore = scoreBuff;
                        fieldHolder.Tiles[j, i].TileState = TileState.Occupied;
                    }
                }

                fullTileIndex = new List<int>(fieldHolder.AxisLength);
                Aligning();


                // Merging stage

                for (int j = 0; j < fullTileIndex.Count - 1; ++j)
                {
                    if (fieldHolder.Tiles[j, i].TileScore != fieldHolder.Tiles[j + 1, i].TileScore)
                        continue;

                    if (fieldHolder.Tiles[j, i].TileState == TileState.Free)
                        continue;

                    ++scoreResult;

                    fieldHolder.Tiles[j, i].TileScore *= 2;
                    fieldHolder.Tiles[j + 1, i].TileScore = 0;
                    fieldHolder.Tiles[j + 1, i].TileState = TileState.Free;
                }


                fullTileIndex = new List<int>(fieldHolder.AxisLength);
                Aligning();
            }
        }
        void RightSwipe()
        {
            for (int i = 0; i < fieldHolder.AxisLength; ++i)
            {
                void Aligning()
                {
                    for (int j = fieldHolder.AxisLength - 1; j >= 0; --j)
                        if (fieldHolder.Tiles[i, j].TileState == TileState.Occupied)
                            fullTileIndex.Add(j);

                    for (int j = 0; j < fullTileIndex.Count; ++j)
                    {
                        int scoreBuff = fieldHolder.Tiles[i, fullTileIndex[j]].TileScore;

                        fieldHolder.Tiles[i, fullTileIndex[j]].TileState = TileState.Free;
                        fieldHolder.Tiles[i, fullTileIndex[j]].TileScore = 0;

                        fieldHolder.Tiles[i, fieldHolder.AxisLength - 1 - j].TileScore = scoreBuff;
                        fieldHolder.Tiles[i, fieldHolder.AxisLength - 1 - j].TileState = TileState.Occupied;
                    }
                }

                fullTileIndex = new List<int>(fieldHolder.AxisLength);
                Aligning();


                // Merging

                for (int j = fieldHolder.AxisLength - 1; j > fieldHolder.AxisLength - fullTileIndex.Count; --j)
                {
                    if (fieldHolder.Tiles[i, j].TileScore != fieldHolder.Tiles[i, j - 1].TileScore)
                        continue;

                    if (fieldHolder.Tiles[i, j].TileState == TileState.Free)
                        continue;

                    ++scoreResult;

                    fieldHolder.Tiles[i, j].TileScore *= 2;
                    fieldHolder.Tiles[i, j - 1].TileScore = 0;
                    fieldHolder.Tiles[i, j - 1].TileState = TileState.Free;
                }

                fullTileIndex = new List<int>(fieldHolder.AxisLength);
                Aligning();
            }
        }
        void DownSwipe()
        {
            for (int i = 0; i < fieldHolder.AxisLength; ++i)
            {
                void Aligning()
                {
                    for (int j = fieldHolder.AxisLength - 1; j >= 0; --j)
                        if (fieldHolder.Tiles[j, i].TileState == TileState.Occupied)
                            fullTileIndex.Add(j);

                    for (int j = 0; j < fullTileIndex.Count; ++j)
                    {
                        int scoreBuff = fieldHolder.Tiles[fullTileIndex[j], i].TileScore;

                        fieldHolder.Tiles[fullTileIndex[j], i].TileState = TileState.Free;
                        fieldHolder.Tiles[fullTileIndex[j], i].TileScore = 0;

                        fieldHolder.Tiles[fieldHolder.AxisLength - 1 - j, i].TileScore = scoreBuff;
                        fieldHolder.Tiles[fieldHolder.AxisLength - 1 - j, i].TileState = TileState.Occupied;
                    }
                }

                fullTileIndex = new List<int>(fieldHolder.AxisLength);
                Aligning();


                // Merging

                for (int j = fieldHolder.AxisLength - 1; j > fieldHolder.AxisLength - fullTileIndex.Count; --j)
                {
                    if (fieldHolder.Tiles[j, i].TileScore != fieldHolder.Tiles[j - 1, i].TileScore)
                        continue;

                    if (fieldHolder.Tiles[j, i].TileState == TileState.Free)
                        continue;

                    ++scoreResult;

                    fieldHolder.Tiles[j, i].TileScore *= 2;
                    fieldHolder.Tiles[j - 1, i].TileScore = 0;
                    fieldHolder.Tiles[j - 1, i].TileState = TileState.Free;
                }

                fullTileIndex = new List<int>(fieldHolder.AxisLength);
                Aligning();
            }
        }

        return scoreResult;
    }
}
