using UnityEngine;

public class PlayerHealth : HealthManager
{
    private Player player; 

    public bool isDead { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    
        player = GetComponentInParent<Player>();
    }

    public override void ReduceHealth()
    {
        base.ReduceHealth();

        if (ShouldDie())
            Die();
    }

    private void Die()
    {
        isDead = true;
        player.animator.enabled = false;
    }
}
