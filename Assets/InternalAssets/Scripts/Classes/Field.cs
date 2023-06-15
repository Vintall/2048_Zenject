using System.Collections.Generic;
using UnityEngine;


public class Field : MonoBehaviour, IField
{
    [SerializeField]
    Tile[] tiles = new Tile[9];
    int[,] tilesGrid;


    public int FreeTilesCount
    {
        get 
        {
            int count = 0;

            for (int i = 0; i < tiles.Length; ++i)
                    if (tiles[i].GetTileState == TileState.Free)
                        ++count;

            return count;
        }
    }
    static int MaxTilesSpawn = 3;
    static int MinTilesSpawn = 2;

    // Do no change untill procedural generation of field
    const int axisLength = 3; 

    private void Awake()
    {
        tilesGrid = new int[axisLength, axisLength];

        for (int i = 0; i < axisLength; ++i)
            for (int j = 0; j < axisLength; ++j)
                tilesGrid[i, j] = j + i * axisLength;
    }
    public void SpawnTiles()
    {
        if (FreeTilesCount == 0)
            return;

        int tilesToSpawn = Random.Range(MinTilesSpawn, 1 + Mathf.Min(FreeTilesCount, MaxTilesSpawn));

        if (tilesToSpawn > FreeTilesCount)
            tilesToSpawn = FreeTilesCount;

        List<int> freeTileIndexes = new List<int>();

        for (int i = 0; i < tiles.Length; ++i)
            if (tiles[i].GetTileState == TileState.Free)
                freeTileIndexes.Add(i);

        int[] tilesToSpawnIndexes = new int[tilesToSpawn];
        for (int i = 0; i < tilesToSpawn; ++i)
        {
            int index = Random.Range(0, freeTileIndexes.Count);
            tilesToSpawnIndexes[i] = freeTileIndexes[index];
            freeTileIndexes.RemoveAt(index);

            tiles[tilesToSpawnIndexes[i]].TileScore = 2;
            tiles[tilesToSpawnIndexes[i]].GetTileState = TileState.Occupied;
        }

    }
    public bool CheckField()
    {
        bool isProceedPossible = false;
        for (int i = 0; i < axisLength; ++i)
            for (int j = 0; j < axisLength; ++j)
            {
                if (i < axisLength - 1)
                    if (tiles[tilesGrid[i, j]].TileScore == tiles[tilesGrid[i + 1, j]].TileScore)
                    {
                        isProceedPossible = true;
                        break;
                    }
                if (j < axisLength - 1)
                    if (tiles[tilesGrid[i, j]].TileScore == tiles[tilesGrid[i, j + 1]].TileScore)
                    {
                        isProceedPossible = true;
                        break;
                    }
                if(tiles[tilesGrid[i, j]].GetTileState == TileState.Free)
                {
                    isProceedPossible = true;
                    break;
                }
            }
        return isProceedPossible;
    }
    public int InputHandle(SwipeDirection swipeDirection)
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
            for (int i = 0; i < axisLength; ++i)
            {
                void Aligning()
                {
                    for (int j = 0; j < axisLength; ++j)
                        if (tiles[tilesGrid[i, j]].GetTileState == TileState.Occupied)
                            fullTileIndex.Add(j);

                    for (int j = 0; j < fullTileIndex.Count; ++j)
                    {
                        int scoreBuff = tiles[tilesGrid[i, fullTileIndex[j]]].TileScore;

                        tiles[tilesGrid[i, fullTileIndex[j]]].GetTileState = TileState.Free;
                        tiles[tilesGrid[i, fullTileIndex[j]]].TileScore = 0;

                        tiles[tilesGrid[i, j]].TileScore = scoreBuff;
                        tiles[tilesGrid[i, j]].GetTileState = TileState.Occupied;
                    }
                }
                fullTileIndex = new List<int>(axisLength);
                Aligning();



                // Merging
                for (int j = 0; j < fullTileIndex.Count - 1; ++j)
                {
                    if (tiles[tilesGrid[i, j]].TileScore != tiles[tilesGrid[i, j + 1]].TileScore)
                        continue;

                    if (tiles[tilesGrid[i, j]].GetTileState == TileState.Free)
                        continue;

                    ++scoreResult;

                    tiles[tilesGrid[i, j]].TileScore *= 2;
                    tiles[tilesGrid[i, j + 1]].TileScore = 0;
                    tiles[tilesGrid[i, j + 1]].GetTileState = TileState.Free;
                }

                fullTileIndex = new List<int>(axisLength);
                Aligning();
            }
        }
        void UpSwipe()
        {
            for (int i = 0; i < axisLength; ++i) // Columns
            {
                void Aligning()
                {
                    for (int j = 0; j < axisLength; ++j) // Rows
                        if (tiles[tilesGrid[j, i]].GetTileState == TileState.Occupied)
                            fullTileIndex.Add(j);

                    for (int j = 0; j < fullTileIndex.Count; ++j)
                    {
                        int scoreBuff = tiles[tilesGrid[fullTileIndex[j], i]].TileScore;

                        tiles[tilesGrid[fullTileIndex[j], i]].GetTileState = TileState.Free;
                        tiles[tilesGrid[fullTileIndex[j], i]].TileScore = 0;

                        tiles[tilesGrid[j, i]].TileScore = scoreBuff;
                        tiles[tilesGrid[j, i]].GetTileState = TileState.Occupied;
                    }
                }

                fullTileIndex = new List<int>(axisLength);
                Aligning();


                // Merging stage

                for (int j = 0; j < fullTileIndex.Count - 1; ++j)
                {
                    if (tiles[tilesGrid[j, i]].TileScore != tiles[tilesGrid[j + 1, i]].TileScore)
                        continue;

                    if (tiles[tilesGrid[j, i]].GetTileState == TileState.Free)
                        continue;

                    ++scoreResult;

                    tiles[tilesGrid[j, i]].TileScore *= 2;
                    tiles[tilesGrid[j + 1, i]].TileScore = 0;
                    tiles[tilesGrid[j + 1, i]].GetTileState = TileState.Free;
                }


                fullTileIndex = new List<int>(axisLength);
                Aligning();
            }
        }
        void RightSwipe()
        {
            for (int i = 0; i < axisLength; ++i)
            {
                void Aligning()
                {
                    for (int j = axisLength - 1; j >= 0; --j)
                        if (tiles[tilesGrid[i, j]].GetTileState == TileState.Occupied)
                            fullTileIndex.Add(j);

                    for (int j = 0; j < fullTileIndex.Count; ++j)
                    {
                        int scoreBuff = tiles[tilesGrid[i, fullTileIndex[j]]].TileScore;

                        tiles[tilesGrid[i, fullTileIndex[j]]].GetTileState = TileState.Free;
                        tiles[tilesGrid[i, fullTileIndex[j]]].TileScore = 0;

                        tiles[tilesGrid[i, axisLength - 1 - j]].TileScore = scoreBuff;
                        tiles[tilesGrid[i, axisLength - 1 - j]].GetTileState = TileState.Occupied;
                    }
                }

                fullTileIndex = new List<int>(axisLength);
                Aligning();


                // Merging

                for (int j = axisLength - 1; j > axisLength - fullTileIndex.Count; --j)
                {
                    if (tiles[tilesGrid[i, j]].TileScore != tiles[tilesGrid[i, j - 1]].TileScore)
                        continue;

                    if (tiles[tilesGrid[i, j]].GetTileState == TileState.Free)
                        continue;

                    ++scoreResult;

                    tiles[tilesGrid[i, j]].TileScore *= 2;
                    tiles[tilesGrid[i, j - 1]].TileScore = 0;
                    tiles[tilesGrid[i, j - 1]].GetTileState = TileState.Free;
                }

                fullTileIndex = new List<int>(axisLength);
                Aligning();
            }
        }
        void DownSwipe()
        {
            for (int i = 0; i < axisLength; ++i)
            {
                void Aligning()
                {
                    for (int j = axisLength - 1; j >= 0; --j)
                        if (tiles[tilesGrid[j, i]].GetTileState == TileState.Occupied)
                            fullTileIndex.Add(j);

                    for (int j = 0; j < fullTileIndex.Count; ++j)
                    {
                        int scoreBuff = tiles[tilesGrid[fullTileIndex[j], i]].TileScore;

                        tiles[tilesGrid[fullTileIndex[j], i]].GetTileState = TileState.Free;
                        tiles[tilesGrid[fullTileIndex[j], i]].TileScore = 0;

                        tiles[tilesGrid[2 - j, i]].TileScore = scoreBuff;
                        tiles[tilesGrid[2 - j, i]].GetTileState = TileState.Occupied;
                    }
                }

                fullTileIndex = new List<int>(axisLength);
                Aligning();


                // Merging

                for (int j = axisLength - 1; j > axisLength - fullTileIndex.Count; --j)
                {
                    if (tiles[tilesGrid[j, i]].TileScore != tiles[tilesGrid[j - 1, i]].TileScore)
                        continue;

                    if (tiles[tilesGrid[j, i]].GetTileState == TileState.Free)
                        continue;

                    ++scoreResult;

                    tiles[tilesGrid[j, i]].TileScore *= 2;
                    tiles[tilesGrid[j - 1, i]].TileScore = 0;
                    tiles[tilesGrid[j - 1, i]].GetTileState = TileState.Free;
                }

                fullTileIndex = new List<int>(axisLength);
                Aligning();
            }
        }

        return scoreResult;
    }
}
