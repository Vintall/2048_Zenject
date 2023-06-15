using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class Tile : ITile
{
    VisualTile visualTile;
    TileState tileState = TileState.Free;
    int tileScore;
    public int TileScore
    {
        get => tileScore;
        set
        {
            tileScore = value;
            visualTile.TextMesh = tileScore;
        }
    }
    public TileState GetTileState
    {
        get => tileState;
        set => tileState = value;
    }
    public TileState TileState 
    {
        get => tileState;
        set
        {
            tileState = value;
        }
    }
    public Tile(VisualTile visualTile)
    {
        this.visualTile = visualTile;
        tileState = TileState.Free;
        TileScore = 0;
    }
}
