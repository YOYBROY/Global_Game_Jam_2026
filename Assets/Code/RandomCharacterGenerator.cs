using Unity.Mathematics;
using UnityEngine;

public class RandomCharacterGenerator : MonoBehaviour
{
    //Body types
    //Hats
    //Masks

    [SerializeField] private Faketionary bodyTypes;
    [SerializeField] private Faketionary hatTypes;
    [SerializeField] private Faketionary maskTypes;
    [SerializeField] private ColourList clothColour;
    [SerializeField] private ColourList hatColour;
    [SerializeField] private ColourList skinColour;
    [SerializeField] private ColourList maskColour;

    private int bodyIndex;
    private int hatIndex;
    private int maskIndex;
    private int clothColourIndex;
    private int hatColourIndex;
    private int skinColourIndex;
    private int maskColourIndex;

    private GameObject body;
    private GameObject hat;
    private GameObject mask;

    public int ID;

    void Start()
    {
        CalculateIndices();
        CalculateID();
        SpawnModels();
        ApplyColours();
        transform.GetComponent<MeshRenderer>().enabled = false;
    }

    private void CalculateIndices()
    {
        bodyIndex = UnityEngine.Random.Range(0, bodyTypes.things.Length);
        hatIndex = UnityEngine.Random.Range(0, hatTypes.things.Length);
        //maskIndex = Random.Range(0, maskTypes.key.Length);
        clothColourIndex = UnityEngine.Random.Range(0, clothColour.colours.Count);
        hatColourIndex = UnityEngine.Random.Range(0, hatColour.colours.Count);
        skinColourIndex = UnityEngine.Random.Range(0, skinColour.colours.Count);
        maskColourIndex = UnityEngine.Random.Range(0, maskColour.colours.Count);
    }

    private void CalculateID()
    {
        ID = bodyTypes.things[bodyIndex].key * hatTypes.things[hatIndex].key * clothColour.key[clothColourIndex]
            * hatColour.key[hatColourIndex] * skinColour.key[skinColourIndex] * maskColour.key[maskColourIndex]; //* maskTypes.key[maskIndex];
    }

    private void SpawnModels()
    {
        body = Instantiate(bodyTypes.things[bodyIndex].value, transform.position, quaternion.identity, transform);
        Transform hatOffset = body.transform.GetChild(0);
        hat = Instantiate(hatTypes.things[hatIndex].value, hatOffset.position, quaternion.identity, hatOffset);
        //Transform maskOffset = body.transform.GetChild(1);
        //mask = Instantiate(maskTypes.value[maskIndex], maskOffset.position, quaternion.identity, maskOffset);
    }

    private void ApplyColours()
    {
        body.GetComponent<RandomColourPicker>().ApplyColourBasedOnIndex(0, skinColourIndex);
        body.GetComponent<RandomColourPicker>().ApplyColourBasedOnIndex(1, clothColourIndex);
        if(hat.GetComponent<MeshRenderer>() != null)
        {
            hat.GetComponent<RandomColourPicker>().ApplyColourBasedOnIndex(0, hatColourIndex);
        }
        //if(mask.GetComponent<MeshRenderer>() != null)
        //{
            //mask.GetComponent<RandomColourPicker>().ApplyColourBasedOnIndex(0, maskColourIndex);
        //}
    }
}
