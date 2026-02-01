using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotateSpeed = 5f;

    [Header("References")]
    [SerializeField] private InputActionReference moveAction;

    private Vector2 _inputDirection;
    private CharacterController CC;
    private Transform mainCamera;
    Vector3 moveDirection;

    void OnEnable()
    {
        CC = GetComponent<CharacterController>();
        mainCamera = Camera.main.transform;
    }

    void Update()
    {
        MoveDirection();
        Move();
        RotateCharacter();
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
        moveDirection = (forwardMovement + horizontalMovement).normalized;
        CC.SimpleMove(moveDirection * speed);
    }

    void RotateCharacter()
    {
        if(_inputDirection != Vector2.zero)
        {
            Vector3 movement = new Vector3(moveDirection.x, 0, moveDirection.z);
            
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }
}