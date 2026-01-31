using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    //Press interact action
    //get npcs within range of the player
    //Get npc the player is facing

    [SerializeField] private InputActionReference attackAction;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnEnable()
    {
        attackAction.action.started += Attack;
    }

    void OnDisable()
    {
        attackAction.action.started -= Attack;
    }

    void Attack(InputAction.CallbackContext obj)
    {
        Debug.Log("Attacked");
    }
}