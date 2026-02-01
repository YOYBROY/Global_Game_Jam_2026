using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ColourList : ScriptableObject
{
    public int[] key;
    public List<Color> colours = new List<Color>();

    public Color GetRandomColour()
    {
        int randomIndex = Random.Range(0, colours.Count);

        return colours[randomIndex];
    }

    public int GetRandomIndex()
    {
        int randomIndex = Random.Range(0, colours.Count);

        return randomIndex;
    }
}