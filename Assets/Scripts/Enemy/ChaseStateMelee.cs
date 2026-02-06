using UnityEngine;

public class ChaseStateMelee : EnemyState
{
    private EnemyMelee enemy;
    private float lastTimeUpdatedDestination;
    private float updateCooldown = 0.25f;

    public ChaseStateMelee(Enemy enemyBase, EnemyStateMachine stateMachine, string animationBoolName) : base(enemyBase, stateMachine, animationBoolName)
    {
        enemy = enemyBase as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.speed = enemy.chaseSpeed;
        enemy.agent.isStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.transform.rotation = enemy.FaceTarget(GetNextPathPoint());

        if (CanUpdateDestination())
        {
            enemy.agent.destination = enemy.playerTransform.transform.position;
        }
    }

    private bool CanUpdateDestination()
    {
        if (Time.time > lastTimeUpdatedDestination + updateCooldown)
        {
            lastTimeUpdatedDestination = Time.time;
            return true;
        }

        return false;
    }
}
