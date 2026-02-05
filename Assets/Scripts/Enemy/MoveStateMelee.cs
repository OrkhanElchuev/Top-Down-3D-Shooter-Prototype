using UnityEngine;

public class MoveStateMelee : EnemyState
{
    private EnemyMelee enemy;
    private Vector3 destination;

    public MoveStateMelee(Enemy enemyBase, EnemyStateMachine stateMachine, string animationBoolName) : base(enemyBase, stateMachine, animationBoolName)
    {
        enemy = enemyBase as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();

        destination = enemy.GetPatrolDestination();
    }

    public override void Exit()
    {
        base.Exit();

        Debug.Log("Exit Move State");
    }

    public override void Update()
    {
        base.Update();

        enemy.agent.SetDestination(destination);

        if (enemy.agent.remainingDistance <= 1)
            stateMachine.ChangeState(enemy.idleState);
    }
}
