using UnityEngine;

/// <summary>
/// Chase state for a melee enemy.
/// Continuously updates NavMeshAgent destination toward the player at a fixed cooldown,
/// while rotating along the current path.
/// </summary>
public class ChaseStateMelee : EnemyState
{
    #region Fields

    private EnemyMelee enemy;
    private float lastTimeUpdatedDestination;
    private float updateCooldown = 0.25f; // How often the chase destination can be recalculated.

    #endregion

    #region Constructor

    public ChaseStateMelee(Enemy enemyBase, EnemyStateMachine stateMachine, string animationBoolName) : base(enemyBase, stateMachine, animationBoolName)
    {
        enemy = enemyBase as EnemyMelee;
    }

    #endregion

    #region State Lifecycle

    public override void Enter()
    {
        base.Enter();   

        // Chase speed and resume movement.
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

        if (enemy.PlayerInAttackRange())
            stateMachine.ChangeState(enemy.attackState);

        // Rotate along the chase path for smoother cornering.
        enemy.transform.rotation = enemy.FaceTarget(GetNextPathPoint());

        // Refresh destination occasionally to reduce path spam.
        if (CanUpdateDestination())
        {
            enemy.agent.destination = enemy.playerTransform.transform.position;
        }
    }

    #endregion

    #region Helpers

    /// <summary>
    /// Returns true when enough time has passed to refresh the player's destination.
    /// </summary>
    private bool CanUpdateDestination()
    {
        if (Time.time > lastTimeUpdatedDestination + updateCooldown)
        {
            lastTimeUpdatedDestination = Time.time;
            return true;
        }

        return false;
    }

    #endregion
}
