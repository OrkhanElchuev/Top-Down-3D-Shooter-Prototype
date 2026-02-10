using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Base class for enemy states.
/// Provides animation bool handling, a simple state timer, and a utility for reading the next NavMesh path corner.
/// </summary>
public class EnemyState
{
    #region Protected Fields
    
    protected Enemy enemyBase; // Enemy instance this state controls
    protected EnemyStateMachine stateMachine; // State machine that owns this state
    protected string animationBoolName; // Animator bool parameter name used by this state
    protected float stateTimer; // Countdown timer used for idle / cooldown / delays
    protected bool triggerCalled; // True when the animation event for this state has fired

    #endregion

    #region Constructor

    // Creates a new state for an enemy.
    public EnemyState(Enemy enemyBase, EnemyStateMachine stateMachine, string animationBoolName)
    {
        this.enemyBase = enemyBase;
        this.stateMachine = stateMachine;
        this.animationBoolName = animationBoolName;
    }

    #endregion

    #region State Lifecycle

    /// <summary>
    /// Called once when the state becomes active.
    /// Enables the animator bool for this state's animation.
    /// </summary>
    public virtual void Enter()
    {
        enemyBase.animator.SetBool(animationBoolName, true);  

        // Reset animation-trigger flag each time the state is entered.
        triggerCalled = false; 
    }

    /// <summary>
    /// Called once when the state is exited.
    /// Disables the animator bool for this state's animation.
    /// </summary>
    public virtual void Exit()
    {
        enemyBase.animator.SetBool(animationBoolName, false);  
    }

    /// <summary>
    /// Called every frame while this state is active.
    /// Default behavior decreases <see cref="stateTimer"/>.
    /// </summary>
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    #endregion

    #region Animation Events

    // Called by the enemy animation event bridge to signal that a key animation moment happened
    public void AnimationTrigger() => triggerCalled = true;

    #endregion

    #region Navigation Helpers

    /// <summary>
    /// Returns the next point the NavMeshAgent should move/face toward based on its current path.
    /// This is helpful for rotating smoothly along corners instead of always facing the final destination.
    /// </summary>
    protected Vector3 GetNextPathPoint()
    {
        NavMeshAgent agent = enemyBase.agent;
        NavMeshPath path = agent.path;

        // If there are no corners (or only one), just face the destination.
        if (path.corners.Length < 2)
            return agent.destination;
        
        // Find the first corner that is close, then return the next corner.
        for (int i = 0; i < path.corners.Length; i++)
        {
            if (Vector3.Distance(agent.transform.position, path.corners[i]) < 1)
                return path.corners[i + 1];
        }

        return agent.destination;
    }

    #endregion
}
