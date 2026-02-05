using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class EnemyMelee : Enemy
{
    public float stateTimer;

    public IdleStateMelee idleState { get; private set; }
    public MoveStateMelee moveState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleStateMelee(this, stateMachine, "Idle");
        moveState = new MoveStateMelee(this, stateMachine, "Move");
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
