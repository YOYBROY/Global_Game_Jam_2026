using UnityEngine;

public class RandomColourPicker : MonoBehaviour
{
    [SerializeField] private ColourList[] colourList;
    [SerializeField] private bool reverseBodyMaterials;
    private Renderer objectRenderer;

    public void ApplyColourBasedOnIndex(int materialIndex, int colourIndex)
    {
        if(reverseBodyMaterials)
        {
            materialIndex = materialIndex == 0 ? 1 : 0;
        }
        objectRenderer = GetComponent<Renderer>();
        if(objectRenderer.materials.Length == 1)
        {
            objectRenderer.material.color = colourList[materialIndex].colours[colourIndex];
        }
        if(objectRenderer.materials.Length > 1)
        {
            objectRenderer.materials[0].color = colourList[materialIndex].colours[colourIndex];
            objectRenderer.materials[1].color = colourList[materialIndex].colours[colourIndex];
        }
    }

    public void GenerateColours()
    {
        objectRenderer = GetComponent<Renderer>();
        
        Material[] materials = objectRenderer.sharedMaterials;
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = colourList[i].GetRandomColour();
        }
    }
}