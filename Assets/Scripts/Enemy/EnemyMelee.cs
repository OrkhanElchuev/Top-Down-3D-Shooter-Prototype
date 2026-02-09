using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

/// <summary>
/// Enemy implementation for melee behavior.
/// Creates melee states and delegates per-frame logic to the active state.
/// </summary>
public class EnemyMelee : Enemy
{
    #region States

    public IdleStateMelee idleState { get; private set; }
    public MoveStateMelee moveState { get; private set; }
    public RecoveryStateMelee recoveryState { get; private set; }
    public ChaseStateMelee chaseState { get; private set; }
    public AttackStateMelee attackState { get; private set; }

    #endregion

    #region Unity Lifecycle

    protected override void Awake()
    {
        base.Awake();

        // Instantiate all states once, then switch between them.
        idleState = new IdleStateMelee(this, stateMachine, "Idle");
        moveState = new MoveStateMelee(this, stateMachine, "Move");
        recoveryState = new RecoveryStateMelee(this, stateMachine, "Recovery");
        chaseState = new ChaseStateMelee(this, stateMachine, "Chase");
        attackState = new AttackStateMelee(this, stateMachine, "Attack");
    }

    protected override void Start()
    {
        base.Start();
        
        // Start in idle.
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        
        // Let the active state run its per-frame logic.
        stateMachine.currentState.Update();
    }

    #endregion
}
