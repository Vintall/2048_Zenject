using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;


public interface IField
{
    public void SpawnTiles();
    public void CheckField();
    public void InputHandle(SwipeDirection swipeDirection);
}
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

            for (int i = 0; i < 9; ++i)
                    if (tiles[i].GetTileState == Tile.TileState.Free)
                        ++count;

            return count;
        }
    }
    static int MaxTilesSpawn = 3;
    static int MinTilesSpawn = 2;

    private void Awake()
    {
        tilesGrid = new int[3, 3];

        for (int i = 0; i < 3; ++i)
            for (int j = 0; j < 3; ++j)
                tilesGrid[i, j] = j + i * 3;
    }
    public void SpawnTiles()
    {
        if (FreeTilesCount == 0)
            return;

        int tilesToSpawn = Random.Range(MinTilesSpawn, 1 + Mathf.Min(FreeTilesCount, MaxTilesSpawn));
        //Debug.Log($"Tiles to spawn: {tilesToSpawn}");

        if (tilesToSpawn > FreeTilesCount)
            tilesToSpawn = FreeTilesCount;

        List<int> freeTileIndexes = new List<int>();

        for (int i = 0; i < tiles.Length; ++i)
            if (tiles[i].GetTileState == Tile.TileState.Free)
                freeTileIndexes.Add(i);

        int[] tilesToSpawnIndexes = new int[tilesToSpawn];
        for (int i = 0; i < tilesToSpawn; ++i)
        {
            int index = Random.Range(0, freeTileIndexes.Count);
            tilesToSpawnIndexes[i] = freeTileIndexes[index];
            freeTileIndexes.RemoveAt(index);

            tiles[tilesToSpawnIndexes[i]].TileScore = 2;
            tiles[tilesToSpawnIndexes[i]].GetTileState = Tile.TileState.Occupied;
        }

    }
    public void CheckField()
    {
        bool isProceedPossible = false;
        for (int i = 0; i < 3; ++i)
            for (int j = 0; j < 3; ++j)
            {
                if (i < 2)
                    if (tiles[tilesGrid[i, j]].TileScore == tiles[tilesGrid[i + 1, j]].TileScore)
                    {
                        isProceedPossible = true;
                        break;
                    }
                if (j < 2)
                    if (tiles[tilesGrid[i, j]].TileScore == tiles[tilesGrid[i, j + 1]].TileScore)
                    {
                        isProceedPossible = true;
                        break;
                    }
                if(tiles[tilesGrid[i, j]].GetTileState == Tile.TileState.Free)
                {
                    isProceedPossible = true;
                    break;
                }
            }
        if(!isProceedPossible)
        {
            Debug.Log("GameOverEvent");
            return;
        }

        
    }
    public void InputHandle(SwipeDirection swipeDirection)
    { // Definitely require refactoring. But it's enough for some functional state
        CheckField();

        if (swipeDirection == SwipeDirection.Left)
            for (int i = 0; i < 3; ++i)
            {
                List<int> fullTileIndex = new List<int>(3);

                // Aligning
                for (int j = 0; j < 3; ++j)
                    if (tiles[tilesGrid[i, j]].GetTileState == Tile.TileState.Occupied)
                        fullTileIndex.Add(j);

                for (int j = 0; j < fullTileIndex.Count; ++j)
                {
                    int scoreBuff = tiles[tilesGrid[i, fullTileIndex[j]]].TileScore;

                    tiles[tilesGrid[i, fullTileIndex[j]]].GetTileState = Tile.TileState.Free;
                    tiles[tilesGrid[i, fullTileIndex[j]]].TileScore = 0;

                    tiles[tilesGrid[i, j]].TileScore = scoreBuff;
                    tiles[tilesGrid[i, j]].GetTileState = Tile.TileState.Occupied;
                }

                // Merging
                for (int j = 0; j < fullTileIndex.Count - 1; ++j)
                {
                    if (tiles[tilesGrid[i, j]].TileScore != tiles[tilesGrid[i, j + 1]].TileScore)
                        continue;

                    if (tiles[tilesGrid[i, j]].GetTileState == Tile.TileState.Free)
                        continue;

                    tiles[tilesGrid[i, j]].TileScore *= 2;
                    tiles[tilesGrid[i, j + 1]].TileScore = 0;
                    tiles[tilesGrid[i, j + 1]].GetTileState = Tile.TileState.Free;
                }

                // Finalising
                fullTileIndex = new List<int>(3);

                for (int j = 0; j < 3; ++j)
                    if (tiles[tilesGrid[i, j]].GetTileState == Tile.TileState.Occupied)
                        fullTileIndex.Add(j);

                for (int j = 0; j < fullTileIndex.Count; ++j)
                {
                    int scoreBuff = tiles[tilesGrid[i, fullTileIndex[j]]].TileScore;

                    tiles[tilesGrid[i, fullTileIndex[j]]].GetTileState = Tile.TileState.Free;
                    tiles[tilesGrid[i, fullTileIndex[j]]].TileScore = 0;

                    tiles[tilesGrid[i, j]].TileScore = scoreBuff;
                    tiles[tilesGrid[i, j]].GetTileState = Tile.TileState.Occupied;
                }
            }

        if (swipeDirection == SwipeDirection.Up)
            for (int i = 0; i < 3; ++i) // Columns
            {
                List<int> fullTileIndex = new List<int>(3);

                for (int j = 0; j < 3; ++j) // Rows
                    if (tiles[tilesGrid[j, i]].GetTileState == Tile.TileState.Occupied)
                        fullTileIndex.Add(j);

                for (int j = 0; j < fullTileIndex.Count; ++j)
                {
                    int scoreBuff = tiles[tilesGrid[fullTileIndex[j], i]].TileScore;

                    tiles[tilesGrid[fullTileIndex[j], i]].GetTileState = Tile.TileState.Free;
                    tiles[tilesGrid[fullTileIndex[j], i]].TileScore = 0;

                    tiles[tilesGrid[j, i]].TileScore = scoreBuff;
                    tiles[tilesGrid[j, i]].GetTileState = Tile.TileState.Occupied;
                }

                // Merging stage

                for (int j = 0; j < fullTileIndex.Count - 1; ++j)
                {
                    if (tiles[tilesGrid[j, i]].TileScore != tiles[tilesGrid[j + 1, i]].TileScore)
                        continue;

                    if (tiles[tilesGrid[j, i]].GetTileState == Tile.TileState.Free)
                        continue;

                    tiles[tilesGrid[j, i]].TileScore *= 2;
                    tiles[tilesGrid[j + 1, i]].TileScore = 0;
                    tiles[tilesGrid[j + 1, i]].GetTileState = Tile.TileState.Free;
                }


                // Finalising

                fullTileIndex = new List<int>(3);

                for (int j = 0; j < 3; ++j) // Rows
                    if (tiles[tilesGrid[j, i]].GetTileState == Tile.TileState.Occupied)
                        fullTileIndex.Add(j);

                for (int j = 0; j < fullTileIndex.Count; ++j)
                {
                    int scoreBuff = tiles[tilesGrid[fullTileIndex[j], i]].TileScore;

                    tiles[tilesGrid[fullTileIndex[j], i]].GetTileState = Tile.TileState.Free;
                    tiles[tilesGrid[fullTileIndex[j], i]].TileScore = 0;

                    tiles[tilesGrid[j, i]].TileScore = scoreBuff;
                    tiles[tilesGrid[j, i]].GetTileState = Tile.TileState.Occupied;
                }
            }

        if (swipeDirection == SwipeDirection.Right)
            for (int i = 0; i < 3; ++i)
            {
                List<int> fullTileIndex = new List<int>(3);

                for (int j = 2; j >= 0; --j)
                    if (tiles[tilesGrid[i, j]].GetTileState == Tile.TileState.Occupied)
                        fullTileIndex.Add(j);

                for (int j = 0; j < fullTileIndex.Count; ++j)
                {
                    int scoreBuff = tiles[tilesGrid[i, fullTileIndex[j]]].TileScore;

                    tiles[tilesGrid[i, fullTileIndex[j]]].GetTileState = Tile.TileState.Free;
                    tiles[tilesGrid[i, fullTileIndex[j]]].TileScore = 0;

                    tiles[tilesGrid[i, 2 - j]].TileScore = scoreBuff;
                    tiles[tilesGrid[i, 2 - j]].GetTileState = Tile.TileState.Occupied;
                }

                // Merging

                for (int j = 2; j > 3 - fullTileIndex.Count; --j)
                {
                    if (tiles[tilesGrid[i, j]].TileScore != tiles[tilesGrid[i, j - 1]].TileScore)
                        continue;

                    if (tiles[tilesGrid[i, j]].GetTileState == Tile.TileState.Free)
                        continue;

                    tiles[tilesGrid[i, j]].TileScore *= 2;
                    tiles[tilesGrid[i, j - 1]].TileScore = 0;
                    tiles[tilesGrid[i, j - 1]].GetTileState = Tile.TileState.Free;
                }

                // Finalising

                fullTileIndex = new List<int>(3);

                for (int j = 2; j >= 0; --j)
                    if (tiles[tilesGrid[i, j]].GetTileState == Tile.TileState.Occupied)
                        fullTileIndex.Add(j);

                for (int j = 0; j < fullTileIndex.Count; ++j)
                {
                    int scoreBuff = tiles[tilesGrid[i, fullTileIndex[j]]].TileScore;

                    tiles[tilesGrid[i, fullTileIndex[j]]].GetTileState = Tile.TileState.Free;
                    tiles[tilesGrid[i, fullTileIndex[j]]].TileScore = 0;

                    tiles[tilesGrid[i, 2 - j]].TileScore = scoreBuff;
                    tiles[tilesGrid[i, 2 - j]].GetTileState = Tile.TileState.Occupied;
                }
            }

        if (swipeDirection == SwipeDirection.Down)
            for (int i = 0; i < 3; ++i)
            {
                List<int> fullTileIndex = new List<int>(3);

                for (int j = 2; j >= 0; --j)
                    if (tiles[tilesGrid[j, i]].GetTileState == Tile.TileState.Occupied)
                        fullTileIndex.Add(j);

                for (int j = 0; j < fullTileIndex.Count; ++j)
                {
                    int scoreBuff = tiles[tilesGrid[fullTileIndex[j], i]].TileScore;

                    tiles[tilesGrid[fullTileIndex[j], i]].GetTileState = Tile.TileState.Free;
                    tiles[tilesGrid[fullTileIndex[j], i]].TileScore = 0;

                    tiles[tilesGrid[2 - j, i]].TileScore = scoreBuff;
                    tiles[tilesGrid[2 - j, i]].GetTileState = Tile.TileState.Occupied;
                }

                // Merging

                for (int j = 2; j > 3 - fullTileIndex.Count; --j)
                {
                    if (tiles[tilesGrid[j, i]].TileScore != tiles[tilesGrid[j - 1, i]].TileScore)
                        continue;

                    if (tiles[tilesGrid[j, i]].GetTileState == Tile.TileState.Free)
                        continue;

                    tiles[tilesGrid[j, i]].TileScore *= 2;
                    tiles[tilesGrid[j - 1, i]].TileScore = 0;
                    tiles[tilesGrid[j - 1, i]].GetTileState = Tile.TileState.Free;
                }

                // Finalising
                fullTileIndex = new List<int>(3);

                for (int j = 2; j >= 0; --j)
                    if (tiles[tilesGrid[j, i]].GetTileState == Tile.TileState.Occupied)
                        fullTileIndex.Add(j);

                for (int j = 0; j < fullTileIndex.Count; ++j)
                {
                    int scoreBuff = tiles[tilesGrid[fullTileIndex[j], i]].TileScore;

                    tiles[tilesGrid[fullTileIndex[j], i]].GetTileState = Tile.TileState.Free;
                    tiles[tilesGrid[fullTileIndex[j], i]].TileScore = 0;

                    tiles[tilesGrid[2 - j, i]].TileScore = scoreBuff;
                    tiles[tilesGrid[2 - j, i]].GetTileState = Tile.TileState.Occupied;
                }
            }


        SpawnTiles();
    }
}
