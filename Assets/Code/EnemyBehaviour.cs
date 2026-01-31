using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    //Patrol between points and smoothly turn around, can have times where it just sort of looks left and right before moving to next point.
    //If the player is detected, move towards the player and send an alert signal to Enemys within range.
    //patrol mode, attack mode.

    [Header("Variables")]
    [SerializeField] private Transform pointParent;
    [SerializeField] private bool goToRandomWaypoint;
    [SerializeField] private float stoppedTime = 2f;
    [SerializeField] private float stoppedTimeVariance = 0.5f;
    [SerializeField] private float attackSpeed = 5f;
    [SerializeField] private float killRange = 5f;

    private Transform[] points;
    private bool idling;
    private float speed;
    private int targetPointIndex;
    private GameObject alertTarget;

    public enum EnemyStatus
    {
        IDLE,
        PATROLLING,
        ATTACKING,
    }

    public EnemyStatus status;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
        foreach(Transform point in points)
        {
            point.transform.SetParent(null);
        }
        if(GameEvents.current != null)
        {
            GameEvents.current.onPlayerLocated += AlertEnemy;
        }
        else
        {
            Debug.LogError("GameEvents is not in the scene and event was not added, Please add 'GameEssentials' to the scene");
        }
    }

    void Update()
    {
        Action();
    }

    void Action()
    {
        switch (status)
        {
            case EnemyStatus.IDLE:
                if (idling)
                {
                    return;
                }
                float randomisedTime = stoppedTime + Random.Range(-stoppedTimeVariance, stoppedTimeVariance);
                StartCoroutine(Idle(randomisedTime));
                break;
            case EnemyStatus.PATROLLING:
                Move();
                break;
            case EnemyStatus.ATTACKING:
                agent.speed = attackSpeed;
                Vector3 targetPosition = alertTarget.transform.position;
                targetPosition.y = 0;
                agent.SetDestination(targetPosition);
                float distanceToTarget = (transform.position - targetPosition).magnitude;
                if(distanceToTarget < killRange)
                {
                    Debug.Log("Game Over");
                }
                break;
        }
    }

    private IEnumerator Idle(float duration)
    {
        agent.speed = 0;
        idling = true;
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        FinishIdle();
    }

    private void FinishIdle()
    {
        agent.speed = speed;
        status = EnemyStatus.PATROLLING;
        idling = false;
    }

    private void Move()
    {
        if (agent.remainingDistance <= agent.stoppingDistance + agent.baseOffset)
        {
            status = EnemyStatus.IDLE;
            ProgressWaypoints();
        }
    }

    public void AlertEnemy(GameObject target)
    {
        status = EnemyStatus.ATTACKING;
        alertTarget = target;
    }

    void ProgressWaypoints()
    {
        if(goToRandomWaypoint)
        {
            int randomIndex = Random.Range(0, points.Length);
            agent.SetDestination(new Vector3(points[randomIndex].position.x, 0, points[randomIndex].position.z));
            targetPointIndex = randomIndex;
        }
        else
        {
            targetPointIndex ++;
            targetPointIndex %= points.Length;
            Vector3 newTargetPoint = points[targetPointIndex].position;
            newTargetPoint.y = 0;
            agent.SetDestination(newTargetPoint);
        }
    }

    void OnDestroy()
    {
        GameEvents.current.onPlayerLocated -= AlertEnemy;
    }
}