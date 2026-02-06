using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class EnemyMelee : Enemy
{
    private float stateTimer;

    public IdleStateMelee idleState { get; private set; }
    public MoveStateMelee moveState { get; private set; }
    public RecoveryStateMelee recoveryState { get; private set; }
    public ChaseStateMelee chaseState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleStateMelee(this, stateMachine, "Idle");
        moveState = new MoveStateMelee(this, stateMachine, "Move");
        recoveryState = new RecoveryStateMelee(this, stateMachine, "Recovery");
        chaseState = new ChaseStateMelee(this, stateMachine, "Chase");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        stateTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.V))
            stateTimer = 3;
    }
}
