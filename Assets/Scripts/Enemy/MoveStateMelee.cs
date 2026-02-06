using UnityEngine;
using UnityEngine.AI;

public class MoveStateMelee : EnemyState
{
    private EnemyMelee enemy;
    private Vector3 destination;

    private float stoppingOffset = 0.05f;

    public MoveStateMelee(Enemy enemyBase, EnemyStateMachine stateMachine, string animationBoolName) : base(enemyBase, stateMachine, animationBoolName)
    {
        enemy = enemyBase as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();

        destination = enemy.GetPatrolDestination();
        enemy.agent.SetDestination(destination);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerInAggressionRange())
        {
            stateMachine.ChangeState(enemy.recoveryState);
            return;
        }

        enemy.transform.rotation = enemy.FaceTarget(GetNextPathPoint());

        if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance + stoppingOffset)
            stateMachine.ChangeState(enemy.idleState);
    }

    private Vector3 GetNextPathPoint()
    {
        NavMeshAgent agent = enemy.agent;
        NavMeshPath path = agent.path;

        if (path.corners.Length < 2)
            return agent.destination;
        
        for (int i = 0; i < path.corners.Length; i++)
        {
            if (Vector3.Distance(agent.transform.position, path.corners[i]) < 1)
                return path.corners[i + 1];
        }

        return agent.destination;
    }
}
