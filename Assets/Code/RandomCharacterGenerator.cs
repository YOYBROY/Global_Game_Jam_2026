using System.Collections.Generic;
using UnityEngine;




public class RandomCharacterGenerator : MonoBehaviour
{
    //Body types
    //Hats (remember bald
    //Masks

    [SerializeField] private Dictionary<GameObject, int> BodyDictionary = new Dictionary<GameObject, int>();
    [SerializeField] public Faketionary bodyTypes;
    [SerializeField] public Faketionary hatTypes;
    [SerializeField] public Faketionary maskTypes;

    private int bodyIndex;
    private int hatIndex;
    private int maskIndex;

    [SerializeField] private GameObject body;
    [SerializeField] private GameObject hat;
    [SerializeField] private GameObject mask;

    public int ID;

    void Start()
    {
        bodyIndex = Random.Range(0, bodyTypes.key.Length);
        hatIndex = Random.Range(0, hatTypes.key.Length);
        maskIndex = Random.Range(0, maskTypes.key.Length);
        ID = bodyTypes.key[bodyIndex] + hatTypes.key[hatIndex] + maskTypes.key[maskIndex];
        body = bodyTypes.value[bodyIndex];
        hat = bodyTypes.value[hatIndex];
        mask = bodyTypes.value[maskIndex];
    }

    void Update()
    {
        
    }
}
