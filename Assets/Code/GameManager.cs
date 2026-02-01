using System.Threading;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    public float escapeTimer;
    [SerializeField] private GameObject player;
    [SerializeField]private GameObject escapeTrigger;

    public enum GameState
    {
        SEARCHING,
        ESCAPING,
        LOSE_STATE,
        WIN_STATE,
    }

    public GameState gameState;

    private Vector3 escapeTriggerSpawn;


    void Start()
    {
        escapeTriggerSpawn = player.transform.position;
        if (GameEvents.current != null)
        {
            GameEvents.current.onNPCKilled += CheckDeadBodyID;
            GameEvents.current.onGameWin += WinGame;
            GameEvents.current.onGameOver += LoseGame;
        }
        else
        {
            Debug.LogError("GameEvents is not in the scene and event was not added, Please add 'GameEssentials' to the scene");
        }
        Time.timeScale = 1;
    }

    void Update()
    {
        if(gameState == GameState.ESCAPING)
        {
            EscapeTimer();
        }
    }
    void WinGame()
    {
        gameState = GameState.WIN_STATE;
    }

    void LoseGame()
    {
        gameState = GameState.LOSE_STATE;
    }

    void EscapeTimer()
    {
        escapeTimer -= Time.deltaTime;
        if(escapeTimer < 0)
        {
            gameState = GameState.LOSE_STATE;
            GameEvents.current.GameOver();
        }
    }

    public void CheckDeadBodyID(GameObject deadBody)
    {
        if (deadBody.GetComponentInChildren<NPC_Behaviour>().levelTarget)
        {
            TriggerEscapeState();
        }
        else
        {
            GameEvents.current.GameOver();
        }
    }

    void TriggerEscapeState()
    {
        gameState = GameState.ESCAPING;
        //start escape timer

        //show escape timer hud
        
        //enable escape trigger
        escapeTrigger = Instantiate(escapeTrigger, escapeTriggerSpawn, quaternion.identity);
    }

    void OnDestroy()
    {
        GameEvents.current.onNPCKilled -= CheckDeadBodyID;
            GameEvents.current.onGameWin -= WinGame;
            GameEvents.current.onGameOver -= LoseGame;
    }
}