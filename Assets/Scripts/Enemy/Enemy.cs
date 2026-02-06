using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour
{
    [Header("Idle Settings")]
    public float idleTime;

    [Header("Move Settings")]
    [SerializeField] private Transform[] patrolPoints;
    public float moveSpeed;
    public float chaseSpeed;
    public float turnSpeed;
    
    
    [Header("Behaviour Settings")]
    public Transform playerTransform;
    public float aggressionRange;
    
    private int currentPatrolIndex;

    public Animator animator { get; private set; }
    public NavMeshAgent agent { get; private set; }
    public EnemyStateMachine stateMachine { get; private set; }

    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
        InitializePatrolPoints();
    }

    protected virtual void Update()
    {
        
    }

    public Vector3 GetPatrolDestination()
    {
        Vector3 destination = patrolPoints[currentPatrolIndex].transform.position;

        currentPatrolIndex++;

        if (currentPatrolIndex >= patrolPoints.Length)
            currentPatrolIndex = 0;
        
        return destination;
    }

    public Quaternion FaceTarget(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
        Vector3 currentEulerAngles = transform.rotation.eulerAngles;

        float yRotation = Mathf.LerpAngle(currentEulerAngles.y, targetRotation.eulerAngles.y, turnSpeed * Time.deltaTime);

        return Quaternion.Euler(currentEulerAngles.x, yRotation, currentEulerAngles.z);
    }

    public bool PlayerInAggressionRange() => Vector3.Distance(transform.position, playerTransform.position) < aggressionRange;
    public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();

    private void InitializePatrolPoints()
    {
        foreach (Transform t in patrolPoints)
        {
            t.parent = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, aggressionRange);
    }
}
