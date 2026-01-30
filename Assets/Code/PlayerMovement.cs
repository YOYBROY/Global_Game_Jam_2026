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
        Debug.Log(_inputDirection);
        Vector3 forwardMovement = (mainCamera.forward * _inputDirection.y);
        Vector3 horizontalMovement = (mainCamera.right * _inputDirection.x);
        Vector3 moveDirection = (forwardMovement + horizontalMovement).normalized;
        CC.SimpleMove(moveDirection * speed);
    }
}