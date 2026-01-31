using UnityEngine;
using System.Collections.Generic;

public class DecalOnParticleCollision : MonoBehaviour
{
    public GameObject decalPrefab;
    public float decalDisappearTime = 5f;
    private List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void Update()
    {
        
    }

    void OnParticleCollision(GameObject other)
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        int numCollisionEvents = ps.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            Vector3 collisionHitLoc = collisionEvents[i].intersection;
            Vector3 collisionNormal = collisionEvents[i].normal;

            Quaternion decalRotation = Quaternion.LookRotation(collisionNormal);
            GameObject newDecal = Instantiate(decalPrefab, collisionHitLoc, Quaternion.Euler (decalRotation.eulerAngles + new Vector3(180, 0, 0)));

            Destroy(newDecal, decalDisappearTime);
        }
    }
}