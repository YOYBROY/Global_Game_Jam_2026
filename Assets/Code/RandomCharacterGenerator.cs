using Unity.Mathematics;
using UnityEngine;

public class RandomCharacterGenerator : MonoBehaviour
{
    [SerializeField] private Faketionary bodyTypes;
    [SerializeField] private Faketionary hatTypes;
    [SerializeField] private Faketionary maskTypes;
    [SerializeField] private ColourList clothColour;
    [SerializeField] private ColourList hatColour;
    [SerializeField] private ColourList skinColour;
    [SerializeField] private ColourList maskColour;

    [SerializeField] bool manualCharacter;

    [SerializeField] private int bodyIndex;
    [SerializeField] private int hatIndex;
    [SerializeField] private int maskIndex;
    [SerializeField] private int clothColourIndex;
    [SerializeField] private int hatColourIndex;
    [SerializeField] private int skinColourIndex;
    [SerializeField] private int maskColourIndex;

    private GameObject body;
    private GameObject hat;
    private GameObject mask;

    public int ID;

    void Start()
    {
        if(!manualCharacter)
        {
            CalculateIndices();
        }
        CalculateID();
        SpawnModels();
        ApplyColours();
        transform.GetComponent<MeshRenderer>().enabled = false;
    }

    private void CalculateIndices()
    {
        bodyIndex = UnityEngine.Random.Range(0, bodyTypes.things.Length);
        hatIndex = UnityEngine.Random.Range(0, hatTypes.things.Length);
        maskIndex = UnityEngine.Random.Range(0, maskTypes.things.Length);
        clothColourIndex = UnityEngine.Random.Range(0, clothColour.colours.Count);
        hatColourIndex = UnityEngine.Random.Range(0, hatColour.colours.Count);
        skinColourIndex = UnityEngine.Random.Range(0, skinColour.colours.Count);
        maskColourIndex = UnityEngine.Random.Range(0, maskColour.colours.Count);
    }

    private void CalculateID()
    {
        ID = bodyTypes.things[bodyIndex].key;
        ID *= hatTypes.things[hatIndex].key;
        ID *= maskTypes.things[maskIndex].key;
        ID *= clothColour.key[clothColourIndex];
        ID *= hatColour.key[hatColourIndex]; 
        ID *= skinColour.key[skinColourIndex]; 
        ID *= maskColour.key[maskColourIndex]; 
    }

    private void SpawnModels()
    {
        body = Instantiate(bodyTypes.things[bodyIndex].value, transform.position, quaternion.identity, transform);
        Transform accessoryOffset = body.transform.GetChild(0);
        hat = Instantiate(hatTypes.things[hatIndex].value, accessoryOffset.position, quaternion.identity, accessoryOffset);
        mask = Instantiate(maskTypes.things[maskIndex].value, accessoryOffset.position, quaternion.identity, accessoryOffset);
    }

    private void ApplyColours()
    {
        body.GetComponent<RandomColourPicker>().ApplyColourBasedOnIndex(0, clothColourIndex);
        body.GetComponent<RandomColourPicker>().ApplyColourBasedOnIndex(1, skinColourIndex);
        if(hat.GetComponent<MeshRenderer>() != null)
        {
            hat.GetComponent<RandomColourPicker>().ApplyColourBasedOnIndex(0, hatColourIndex);
        }
        if(mask.GetComponent<MeshRenderer>() != null)
        {
            mask.GetComponent<RandomColourPicker>().ApplyColourBasedOnIndex(0, maskColourIndex);
        }
    }
}
