using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

/// <summary>
/// Base enemy class containing shared data and utilities:
/// </summary>

public class Enemy : MonoBehaviour
{
    public LayerMask whatIsPlayer; 

    #region Inspector: Idle Settings

    [Header("Idle Settings")]
    [Tooltip("How long the enemy stays in the idle state before switching back to patrol movement.")]
    public float idleTime;

    #endregion

    #region Inspector: Move Settings

    [Header("Move Settings")]
    [Tooltip("World-space patrol points. The enemy will cycle through them in order.")]
    [SerializeField] private Transform[] patrolPoints;

    [Tooltip("NavMesh movement speed used while patrolling.")]
    public float moveSpeed;

    [Tooltip("NavMesh movement speed used while chasing the player.")]
    public float chaseSpeed;

    [Tooltip("How quickly the enemy rotates to face its target direction.")]
    public float turnSpeed;

    private bool manualMovement;

    #endregion

    #region Inspector: Attack Settings

    [Header("Attack Settings")]
    public float attackRange;
    public float attackMoveSpeed;

    #endregion

    #region Inspector: Behaviour Settings

    [Header("Behaviour Settings")]
    [Tooltip("Reference to the player's transform (used for detection and chasing).")]
    public Transform playerTransform;

    [Tooltip("Distance at which the enemy becomes aggressive and transitions into chase behavior.")]
    public float aggressionRange;

    #endregion

    #region Private Variables

    private Vector3[] patrolPointsPositions;
    private int currentPatrolIndex;

    #endregion

    #region Components / Systems

    // Animator for state driven animations.
    public Animator animator { get; private set; }

    // Navmesh for navigation and pathfinding.
    public NavMeshAgent agent { get; private set; }

    // Finite state machine controlling enemy behavior.
    public EnemyStateMachine stateMachine { get; private set; }

    public bool inBattleMode { get; private set; }

    public EnemyHealth health { get; private set; }

    #endregion

    #region Unity Lifecycle

    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        health = GetComponent<EnemyHealth>();
    }

    protected virtual void Start()
    {
        InitializePatrolPoints();
    }

    protected virtual void Update()
    {
        
    }

    #endregion

    #region Helper Methods

    public virtual void GetHit()
    {
        health.ReduceHealth();
        
        if(health.ShouldDie())
            Die();

        EnterBattleMode();
    }

    public virtual void Die()
    {
        
    }

    public virtual void EnterBattleMode()
    {
        inBattleMode = true;
    }

    protected bool ShouldEnterBattleMode()
    {
        bool inAggressionRange = Vector3.Distance(transform.position, playerTransform.position) < aggressionRange;

        if (inAggressionRange && !inBattleMode)
        {
            EnterBattleMode();
            return true;
        }
        return false;
    }

    public bool PlayerInAttackRange() => Vector3.Distance(transform.position, playerTransform.position) < attackRange;

    // Called by animation event script to forward events to the active state.
    public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, aggressionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    #endregion

    #region Movement / Targeting 
    
    /// <summary>
    /// Returns the next patrol point position and advances the patrol index (loops at the end).
    /// </summary>
    public Vector3 GetPatrolDestination()
    {
        Vector3 destination = patrolPointsPositions[currentPatrolIndex];

        currentPatrolIndex++;

        if (currentPatrolIndex >= patrolPoints.Length)
            currentPatrolIndex = 0;
        
        return destination;
    }

    /// <summary>
    /// Detaches patrol points from this enemy so they remain in world space if the enemy moves/rotates.
    /// </summary>
    private void InitializePatrolPoints()
    {
        patrolPointsPositions = new Vector3[patrolPoints.Length];

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPointsPositions[i] = patrolPoints[i].position;
            patrolPoints[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Smooth rotation that turns toward <paramref name="target"/> on the Y axis.
    /// </summary>
    /// <param name="target">World-space position to face.</param>
    public void FaceTarget(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
        Vector3 currentEulerAngles = transform.rotation.eulerAngles;

        float yRotation = Mathf.LerpAngle(currentEulerAngles.y, targetRotation.eulerAngles.y, turnSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(currentEulerAngles.x, yRotation, currentEulerAngles.z);
    }

    public void ActivateManualMovement(bool manualMovement) => this.manualMovement = manualMovement;
    public bool ManualMovementActive() => manualMovement;

    #endregion
}
