using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Patrol movement state for a melee enemy.
/// Picks the next patrol point, moves to it, rotates along path corners,
/// and returns to idle when the destination is reached.
/// If the player enters aggression range, transitions to recovery.
/// </summary>
public class MoveStateMelee : EnemyState
{
    #region Fields

    private EnemyMelee enemy;
    private Vector3 destination;

    private float stoppingOffset = 0.05f; // Tolerance to avoid jittery behavior caused by Navmesh precision.

    #endregion

    #region Constructor

    public MoveStateMelee(Enemy enemyBase, EnemyStateMachine stateMachine, string animationBoolName) : base(enemyBase, stateMachine, animationBoolName)
    {
        enemy = enemyBase as EnemyMelee;
    }

    #endregion

    #region State Lifecycle

    public override void Enter()
    {
        base.Enter();

        // Patrol speed.
        enemy.agent.speed = enemy.moveSpeed;

        // Choose a patrol destination and begin moving.
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

        // Smoothly rotate in the direction of the next path corner.
        enemy.FaceTarget(GetNextPathPoint());

        // Close enough to destination -> go idle.
        if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance + stoppingOffset)
            stateMachine.ChangeState(enemy.idleState);
    }

    #endregion
}
