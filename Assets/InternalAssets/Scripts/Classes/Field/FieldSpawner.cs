using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class FieldSpawner : MonoBehaviour, IFieldSpawner
{
    [SerializeField]
    float visualSize;

    [SerializeField]
    FieldHolder field;

    [SerializeField]
    Transform visualField;

    [Inject]
    VisualTile.Factory visualTileFactory;

    public ITile[,] SpawnField(IField field, int size)
    {
        Vector3[,] pos = new Vector3[size, size];
        ITile[,] tiles = new ITile[size, size];

        float halfSize = visualSize / 2f;
        for (int i = 0; i < size; ++i)
            for (int j = 0; j < size; ++j)
            {
                float xPos = transform.position.x - halfSize + visualSize * (i + 1) / size - halfSize / size;
                float zPos = transform.position.z - halfSize + visualSize * (j + 1) / size - halfSize / size;
                pos[i, j] = new Vector3(-zPos , 0, xPos);

                VisualTile visualTile = visualTileFactory.Create();

                visualTile.transform.position = new Vector3(
                    pos[i, j].x,
                    0.3f,
                    pos[i, j].z);

                visualTile.transform.localScale = new Vector3(.9f * visualSize / size, 0.2f / size, .9f * visualSize / size);

                visualTile.transform.SetParent(visualField);

                tiles[i, j] = new Tile(visualTile);
            }
        field.Tiles = tiles;
        field.AxisLength = size;

        return tiles;
    }
}
