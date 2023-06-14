using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColorsManager : ITileColorsManager
{
    Dictionary<int, Color> colorDictionary;
    public Color GetColor(int score)
    {
        if (colorDictionary == null)
            colorDictionary = new()
            {
                { 0, new Color(.5f, .5f, .5f) }
            };

        if (!colorDictionary.ContainsKey(score))
        {
            Color newColor = new Color(
                Random.Range(0.3f, 1f),
                Random.Range(0.3f, 1f),
                Random.Range(0.3f, 1f));

            colorDictionary.Add(score, newColor);
        }

        return colorDictionary[score];
    }
}
