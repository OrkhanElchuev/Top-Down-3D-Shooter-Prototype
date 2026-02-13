using UnityEngine;

public class EnemyWeaponModel : MonoBehaviour
{
    public EnemyMeleeWeaponSO weaponSO;

    private bool canDamage;

    private void Awake()
    {
        DisableDamage();
    }

    public void EnableDamage()
    {
        canDamage = true;
    }

    public void DisableDamage()
    {
        canDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canDamage) return;

        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(weaponSO.damageAmount);
            DisableDamage(); // prevents multi-hit per swing
        }
    }
}