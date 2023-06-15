using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(MeshRenderer))]
public class VisualTile : MonoBehaviour
{
    [SerializeField]
    TextMesh textMesh;
    [Inject]
    ITileColorsManager colorsManager;
    MeshRenderer meshRenderer;
    public int TextMesh
    {
        set
        {
            textMesh.text = value == 0 ? "" : $"{value}";
            Color = colorsManager.GetColor(value);
        }
    }
    public Color Color
    {
        set =>
            Material.color = value;
    }
    Material Material
    { 
        get =>
            meshRenderer.material;
    }
    private void Awake() =>
        meshRenderer = GetComponent<MeshRenderer>();

    public class Factory : PlaceholderFactory<VisualTile>
    { }

    [Inject]
    public void Construct(ITileColorsManager colorsManager)
    {
        this.colorsManager = colorsManager;
    }
}

