using UnityEngine;

public class AttackStateMelee : EnemyState
{
    private const float MAX_ATTACK_DISTANCE = 60f;

    private EnemyMelee enemy;
    private Vector3 attackDirection;

    public AttackStateMelee(Enemy enemyBase, EnemyStateMachine stateMachine, string animationBoolName) : base(enemyBase, stateMachine, animationBoolName)
    {
        enemy = enemyBase as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();   

        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;

        attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTANCE);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.manualMovementActive())
        {
            enemy.transform.position = 
                Vector3.MoveTowards(enemy.transform.position, attackDirection, enemy.attackMoveSpeed * Time.deltaTime);
        }


        if (triggerCalled)
            stateMachine.ChangeState(enemy.recoveryState);
    }
}
