using UnityEngine;

public class EnemyHitbox : Hitbox
{
    private Enemy enemy;

    protected override void Awake()
    {
        base.Awake();

        enemy = GetComponentInParent<Enemy>();
    }

    public override void TakeDamage(int damage)
    {
        enemy.GetHit(damage);
    }
}
