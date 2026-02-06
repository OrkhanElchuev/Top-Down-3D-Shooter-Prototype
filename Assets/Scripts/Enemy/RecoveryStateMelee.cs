using UnityEngine;

/// <summary>
/// Recovery state for a melee enemy before it begins chasing.
/// Used to play a "Notice Player" animation.
/// Once an animation event fires (<see cref="EnemyState.AnimationTrigger"/>),
/// transitions to <see cref="ChaseStateMelee"/>.
/// </summary>
public class RecoveryStateMelee : EnemyState
{
    private EnemyMelee enemy;

    #region Constructor

    public RecoveryStateMelee(Enemy enemyBase, EnemyStateMachine stateMachine, string animationBoolName) : base(enemyBase, stateMachine, animationBoolName)
    {
        enemy = enemyBase as EnemyMelee;
    }

    #endregion

    #region State Lifecycle

    public override void Enter()
    {
        base.Enter();

        // Stop moving during recovery state.
        enemy.agent.isStopped = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // Keep facing the player while in recovery state.
        enemy.transform.rotation = enemy.FaceTarget(enemy.playerTransform.position);

        // Wait for animation event to indicate recovery is done.
        if (triggerCalled)
            stateMachine.ChangeState(enemy.chaseState);
    }

    #endregion
}
