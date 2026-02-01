using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    public void Awake()
    {
        current = this;
    }

    public event Action<GameObject> onPlayerLocated;
    public void PlayerLocated(GameObject playerLocation)
    {
        if(onPlayerLocated != null)
        {
            onPlayerLocated(playerLocation);
        }
    }

    public event Action<GameObject> onNPCTargeted;
    public void NPCTargeted(GameObject target)
    {
        if(onNPCTargeted != null)
        {
            onNPCTargeted(target);
        }
    }

    public event Action<GameObject> onNPCKilled;
    public void NPCKilled(GameObject target)
    {
        if(onNPCKilled != null)
        {
            onNPCKilled(target);
        }
    }
}