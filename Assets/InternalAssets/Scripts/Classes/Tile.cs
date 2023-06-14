using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(MeshRenderer))]
public class Tile : MonoBehaviour
{
    MeshRenderer meshRenderer;

    [SerializeField]
    TextMesh textMesh;
    public Material Material => meshRenderer.material;

    int tileScore;
    public int TileScore
    {
        get => tileScore;
        set
        {
            tileScore = value;
            textMesh.text = tileScore.ToString();
            Material.color = colorsManager.GetColor(tileScore);
        }
    }
    [Inject]
    ITileColorsManager colorsManager;

    TileState tileState = TileState.Free;
    public TileState GetTileState
    {
        get => tileState;
        set => tileState = value;
    }

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        tileState = TileState.Free;
        TileScore = 0;
    }
}
