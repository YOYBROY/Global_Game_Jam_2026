using UnityEngine;

public class WinOnTriggerEnter : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GameEvents.current.GameWin();
    }
}