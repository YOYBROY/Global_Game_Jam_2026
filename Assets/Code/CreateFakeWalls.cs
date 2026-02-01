using UnityEngine;

public class CreateFakeWalls : MonoBehaviour
{
    [SerializeField] private Material fakeWallMaterial;

    void Start()
    {
        BoxCollider[] walls = GetComponentsInChildren<BoxCollider>();
        foreach(BoxCollider wall in walls)
        {
            GameObject realWall = wall.gameObject;
            //duplicate and parent to wall
            GameObject fakeWall = Instantiate(realWall, realWall.transform.position, realWall.transform.rotation);
            //apply fakeWallMat
            fakeWall.GetComponent<Renderer>().material = fakeWallMaterial;
            //change layer to default layer
            fakeWall.layer = LayerMask.NameToLayer("Default");
        }
    }
}