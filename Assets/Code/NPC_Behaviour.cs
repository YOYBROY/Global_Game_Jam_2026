using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class NPC_Behaviour : MonoBehaviour
{
    [Header("Variables")]
    public bool levelTarget;
    [SerializeField] private Transform pointParent;
    [SerializeField] private bool goToRandomWaypoint;
    [SerializeField] private float panicRange;
    [SerializeField] private float panicSpeed;
    [SerializeField] private float panicMoveRadius;
    [SerializeField] private float lookAngle = 30;
    [SerializeField] private Ease lookAroundEaseType;
    [SerializeField] private float beginLookAroundTime = 0.4f;
    [SerializeField] private float endLookAroundTime = 0.4f;
    [SerializeField] private float stoppedTime = 2f;
    [SerializeField] private float stoppedTimeVariance = 0.5f;
    [SerializeField] private GameObject targetIcon;
    
    [Header("References")]
    [SerializeField] private GameObject bloodParticles;

    private NavMeshAgent agent;
    private float baseSpeed;
    private Transform[] points;
    private int targetPointIndex;
    private Vector3 startRotation;
    private bool idling;

    private enum NPCStates
    {
        STATIONARY,
        MOVER,
        PANIC,
        LOOKING,
    }

    [SerializeField] private NPCStates currentState;

    void Start()
    {
        targetIcon.SetActive(false);
        startRotation = transform.rotation.eulerAngles;
        agent = GetComponent<NavMeshAgent>();
        baseSpeed = agent.speed;
        if (pointParent != null)
        {
            points = new Transform[pointParent.childCount];
            for (int i = points.Length - 1; i >= 0; i--)
            {
                points[i] = pointParent.GetChild(i);
                points[i].transform.SetParent(null);
            }
        }
        else
        {
            currentState = NPCStates.STATIONARY;
        }

        if (GameEvents.current != null)
        {
            GameEvents.current.onNPCTargeted += CheckIfSelfIsTarget;
            GameEvents.current.onNPCKilled += NPCKilled;
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

    private void Action()
    {
        switch (currentState)
        {
            case NPCStates.STATIONARY:
                //look around periodically
                if(!idling)
                {
                    OnlyLookAround(stoppedTime);
                }
                break;
            case NPCStates.MOVER:
                //move between set out waypoints
                if (agent.remainingDistance <= agent.stoppingDistance + agent.baseOffset)
                {
                    LookAround(stoppedTime);
                    
                }
                break;
            case NPCStates.LOOKING:
                break;
            case NPCStates.PANIC:
                agent.speed = panicSpeed;
                Panic();
                //increase movement speed
                break;
        }
    }

    private void Panic()
    {
        if (agent.remainingDistance <= agent.stoppingDistance + agent.baseOffset)
        {
            SetRandomPosition();
        }
    }

    private bool LookAround(float duration)
    {
        idling = true;
        agent.speed = 0;
        currentState = NPCStates.LOOKING;
        Vector3 forwardRotation = transform.rotation.eulerAngles;
        Vector3 rightRotation = forwardRotation + new Vector3(0.0f, lookAngle, 0.0f);
        Vector3 leftRotation = forwardRotation - new Vector3(0.0f, lookAngle, 0.0f);

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(beginLookAroundTime)
        .Append(transform.DORotate(rightRotation, duration / 4).SetEase(lookAroundEaseType))
        .AppendInterval(endLookAroundTime)
        .Append(transform.DORotate(leftRotation, duration / 2).SetEase(lookAroundEaseType))
        .AppendInterval(endLookAroundTime)
        .Append(transform.DORotate(forwardRotation, duration / 4).SetEase(lookAroundEaseType))
        .AppendCallback(() => { if(currentState != NPCStates.PANIC) { FinishIdle(); }});
        return true;
    }

    private void OnlyLookAround(float duration)
    {
        idling = true;
        agent.speed = 0;
        Vector3 forwardRotation = startRotation;
        Vector3 rightRotation = forwardRotation + new Vector3(0.0f, lookAngle, 0.0f);
        Vector3 leftRotation = forwardRotation - new Vector3(0.0f, lookAngle * 2, 0.0f);

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(beginLookAroundTime)
        .Append(transform.DORotate(leftRotation, duration / 2).SetEase(lookAroundEaseType))
        .AppendInterval(endLookAroundTime)
        .Append(transform.DORotate(forwardRotation, duration / 2).SetEase(lookAroundEaseType))
        .AppendCallback(() => { if(currentState != NPCStates.PANIC) { OnlyLookAround(stoppedTime); }});
    }

    private void FinishIdle()
    {
        agent.speed = baseSpeed;
        currentState = NPCStates.MOVER;
        idling = false;
        ProgressWaypoints();
    }

    void ProgressWaypoints()
    {
        if (goToRandomWaypoint)
        {
            int randomIndex = UnityEngine.Random.Range(0, points.Length);
            if (targetPointIndex == randomIndex)
            {
                ProgressWaypoints();
                return;
            }
            targetPointIndex = randomIndex;
            agent.SetDestination(new Vector3(points[randomIndex].position.x, 0, points[randomIndex].position.z));
        }
        else
        {
            targetPointIndex++;
            targetPointIndex %= points.Length;
            Vector3 newTargetPoint = points[targetPointIndex].position;
            newTargetPoint.y = 0;
            agent.SetDestination(newTargetPoint);
        }
    }

    public void SetRandomPosition()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * panicMoveRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, panicMoveRadius, 1);
        agent.SetDestination(hit.position);
    }

    void CheckIfSelfIsTarget(GameObject target)
    {
        if(gameObject == target)
        {
            targetIcon.SetActive(true);
        }
        else
        {
            targetIcon.SetActive(false);
        }
    }

    void NPCKilled(GameObject killedNPC)
    {
        if (killedNPC == this.gameObject)
        {
            Die();
        }
        else
        {
            ReactToDeath(killedNPC.transform.position);
        }
    }

    void Die()
    {
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        }
        //add force?
        bloodParticles.SetActive(true);
        gameObject.GetComponent<NPC_Behaviour>().enabled = false;
    }

    void ReactToDeath(Vector3 deathPosition)
    {
        Vector3 runDirection = (transform.position - deathPosition).normalized;
        float randomDistance = UnityEngine.Random.Range(2.0f, 5.0f);
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + runDirection * randomDistance, out hit, randomDistance, 1);
        agent.SetDestination(hit.position);
        currentState = NPCStates.PANIC;
    }

    void OnDestroy()
    {
        GameEvents.current.onNPCTargeted -= CheckIfSelfIsTarget;
        GameEvents.current.onNPCKilled -= NPCKilled;
    }
}