using UnityEngine;

/// <summary>
/// Finite state machine for Enemies.
/// Holds the current state and provides state transitions.
/// </summary>
public class EnemyStateMachine
{
    #region Properties

    // Currently active state.
    public EnemyState currentState { get; private set; }

    #endregion

    #region Public Methods

    /// <summary>
    /// Sets the initial state and calls <see cref="EnemyState.Enter"/>.
    /// </summary>
    /// <param name="startState">State that should be entered first.</param>
    public void Initialize(EnemyState startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    /// <summary>
    /// Transitions from the current state to <paramref name="newState"/>.
    /// Calls <see cref="EnemyState.Exit"/> on the old state and <see cref="EnemyState.Enter"/> on the new one.
    /// </summary>
    /// <param name="newState">State to transition into.</param>
    public void ChangeState(EnemyState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    #endregion
}
