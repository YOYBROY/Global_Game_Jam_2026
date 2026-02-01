using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractions : MonoBehaviour
{
    //Press interact action
    //get npcs within range of the player
    //Get npc the player is facing

    [SerializeField] private float visionRange = 3f;
    [SerializeField] private float visionAngle = 20;
    [SerializeField] private int visionConeResolution = 120;

    [SerializeField] private InputActionReference attackAction;

    private GameObject target;

    void OnEnable()
    {
        visionAngle *= Mathf.Deg2Rad;
        attackAction.action.started += Attack;
    }

    void OnDisable()
    {
        attackAction.action.started -= Attack;
    }

    void Update()
    {
        SelectTarget();
    }

    void SelectTarget()
    {
        float Currentangle = -visionAngle / 2;
        float angleIncrement = visionAngle / (visionConeResolution - 1);
        float Sine;
        float Cosine;
        int count = 0;
        for (int i = 0; i < visionConeResolution; i++)
        {
            Sine = Mathf.Sin(Currentangle);
            Cosine = Mathf.Cos(Currentangle);
            Vector3 RaycastDirection = (transform.forward * Cosine) + (transform.right * Sine);
            if (Physics.Raycast(transform.position, RaycastDirection, out RaycastHit hit, visionRange))
            {
                if(hit.collider.gameObject.CompareTag("NPC"))
                {
                    target = hit.collider.gameObject;
                    Debug.Log("NPCTargeted Event Triggered", hit.collider.gameObject);
                    GameEvents.current.NPCTargeted(hit.collider.gameObject);
                    count ++;
                    break;
                }
            }
            Debug.DrawRay(transform.position, RaycastDirection * visionRange, Color.green);
            Currentangle += angleIncrement;
        }
        if (count == 0)
        {
            target = null;
        }
    }

    void Attack(InputAction.CallbackContext obj)
    {
        GameEvents.current.NPCKilled(target);
        target = null;
        Debug.Log("NPCKilled Event Triggered", target);
    }
}