using System;
using System.Collections.Generic;
using DG.Tweening;
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
    [SerializeField] private bool onlyLookAround;
    [SerializeField] private float lookAngle = 30;
    [SerializeField] private Ease lookAroundEaseType;
    [SerializeField] private float beginLookAroundTime = 0.4f;
    [SerializeField] private float endLookAroundTime = 0.4f;
    [SerializeField] private float stoppedTime = 2f;
    [SerializeField] private float stoppedTimeVariance = 0.5f;
    [SerializeField] private float attackSpeed = 5f;
    [SerializeField] private float killRange = 5f;

    private Transform[] points;
    private bool idling;
    private float speed;
    private int targetPointIndex;
    private GameObject alertTarget;
    private Vector3 startRotation;

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
        if(onlyLookAround)
        {
            startRotation = transform.rotation.eulerAngles;
        }
        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
        points = new Transform[pointParent.childCount];
        for(int i = points.Length - 1; i >= 0 ; i--)
        {
            points[i] = pointParent.GetChild(i);
            points[i].transform.SetParent(null);
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
                float randomisedTime = stoppedTime + UnityEngine.Random.Range(-stoppedTimeVariance, stoppedTimeVariance);
                Idle(randomisedTime);
                break;
            case EnemyStatus.PATROLLING:
                CompleteMovement();
                break;
            case EnemyStatus.ATTACKING:
                agent.speed = attackSpeed;
                Vector3 targetPosition = alertTarget.transform.position;
                targetPosition.y = 0;
                agent.SetDestination(targetPosition);
                float distanceToTarget = (transform.position - targetPosition).magnitude;
                if(distanceToTarget < killRange)
                {
                    Debug.Log("Game Over", this);
                }
                break;
        }
    }

    private void Idle(float duration)
    {
        agent.speed = 0;
        idling = true;
        if(onlyLookAround)
        {
            OnlyLookAround(duration);
        }
        else
        {
            LookAround(duration);
        }
    }

    private bool LookAround(float duration)
    {
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
        .AppendCallback(() => { FinishIdle();});
        return true;
    }

    private void OnlyLookAround(float duration)
    {
        Vector3 forwardRotation = startRotation;
        Vector3 rightRotation = forwardRotation + new Vector3(0.0f, lookAngle, 0.0f);
        Vector3 leftRotation = forwardRotation - new Vector3(0.0f, lookAngle * 2, 0.0f);

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(beginLookAroundTime)
        .Append(transform.DORotate(leftRotation, duration / 2).SetEase(lookAroundEaseType))
        .AppendInterval(endLookAroundTime)
        .Append(transform.DORotate(forwardRotation, duration / 2).SetEase(lookAroundEaseType))
        .AppendCallback(() => { OnlyLookAround(duration);});
    }

    private void FinishIdle()
    {
        agent.speed = speed;
        status = EnemyStatus.PATROLLING;
        idling = false;
    }

    private void CompleteMovement()
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
            int randomIndex = UnityEngine.Random.Range(0, points.Length);
            if(targetPointIndex == randomIndex)
            {
                ProgressWaypoints();
                return;
            }
            targetPointIndex = randomIndex;
            agent.SetDestination(new Vector3(points[randomIndex].position.x, 0, points[randomIndex].position.z));
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