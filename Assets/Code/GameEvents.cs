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
}