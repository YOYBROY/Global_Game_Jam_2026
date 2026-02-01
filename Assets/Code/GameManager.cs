using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    public float escapeTimer;

    public enum GameState
    {
        SEARCHING,
        ESCAPING,
        LOSE_STATE,
        WIN_STATE,
    }

    public GameState gameState;



    void Start()
    {
        if (GameEvents.current != null)
        {
            GameEvents.current.onNPCKilled += CheckDeadBodyID;
        }
        else
        {
            Debug.LogError("GameEvents is not in the scene and event was not added, Please add 'GameEssentials' to the scene");
        }
    }

    void Update()
    {
        if(gameState = GameState.ESCAPING)
        {
            EscapeTimer();
        }
    }

    void EscapeTimer()
    {
        escapeTimer -= Time.deltaTime;
        if(escapeTimer < )
    }

    public void CheckDeadBodyID(GameObject deadBody)
    {
        if (deadBody.GetComponentInChildren<NPC_Behaviour>().levelTarget)
        {
            TriggerEscapeState();
        }
    }

    void TriggerEscapeState()
    {
        gameState = GameState.ESCAPING;
        //start escape timer

        //show escape timer hud

        //enable escape trigger
    }
}