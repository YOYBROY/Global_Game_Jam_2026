using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimator : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        //animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float speed = controller.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }
}
