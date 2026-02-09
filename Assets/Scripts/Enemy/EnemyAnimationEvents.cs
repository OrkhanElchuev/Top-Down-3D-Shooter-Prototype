using UnityEngine;

/// <summary>
/// Animation-event relay.
/// Put this on the animated model (or a child) and call its methods from animation events.
/// It forwards the event to the parent <see cref="Enemy"/> which then forwards to the current state.
/// </summary>

public class EnemyAnimationEvents : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    // Called from an Animation Event to signal the current state's trigger.
    public void AnimationTrigger() => enemy.AnimationTrigger();

    public void StartManualMovement() => enemy.ActivateManualMovement(true);
    public void StopManualMovement() => enemy.ActivateManualMovement(false);
}
