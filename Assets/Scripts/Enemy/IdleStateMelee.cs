using UnityEngine;

/// <summary>
/// Idle state for a melee enemy.
/// Waits for a short duration, then transitions to patrol movement,
/// unless the player enters aggression range (then it transitions to recovery).
/// </summary>
public class IdleStateMelee : EnemyState
{
    private EnemyMelee enemy;

    #region Constructor

    public IdleStateMelee(Enemy enemyBase, EnemyStateMachine stateMachine, string animationBoolName) : base(enemyBase, stateMachine, animationBoolName)
    {
        enemy = enemyBase as EnemyMelee;
    }

    #endregion

    #region State Lifecycle

    public override void Enter()
    {
        base.Enter();

        // How long the enemy should remain idle before patrolling again.
        stateTimer = enemyBase.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // If the player is detected, prepare to chase (via recovery state).
        if (enemy.PlayerInAggressionRange())
        {
            stateMachine.ChangeState(enemy.recoveryState);
            return;
        }

        // Timer ended -> start moving to next patrol point.
        if (stateTimer < 0f)
            stateMachine.ChangeState(enemy.moveState);
    }

    #endregion
}

