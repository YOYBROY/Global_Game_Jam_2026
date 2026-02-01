using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimator : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    private void Start()
    {
        agent = GetComponentInChildren<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }
}
