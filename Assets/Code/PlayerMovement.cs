using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float speed = 5f;

    [Header("References")]
    [SerializeField] private InputActionReference moveAction;

    private Vector2 _inputDirection;
    private CharacterController CC;
    private Transform mainCamera;

    void OnEnable()
    {
        CC = GetComponent<CharacterController>();
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        MoveDirection();
        Move();
    }

    void MoveDirection()
    {
        _inputDirection = moveAction.action.ReadValue<Vector2>();
    }

    void Move()
    {
        Vector3 camForward = new Vector3(mainCamera.forward.x, 0, mainCamera.forward.z).normalized;
        Vector3 camRight = new Vector3(mainCamera.right.x, 0, mainCamera.right.z).normalized;
        Vector3 forwardMovement = camForward * _inputDirection.y;
        Vector3 horizontalMovement = camRight * _inputDirection.x;
        Vector3 moveDirection = (forwardMovement + horizontalMovement).normalized;
        CC.SimpleMove(moveDirection * speed);
    }
}