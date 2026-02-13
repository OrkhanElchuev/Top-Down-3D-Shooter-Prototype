using UnityEngine;

public class EnemyWeaponModel : MonoBehaviour
{
    public EnemyMeleeWeaponSO weaponSO;

    [Header("Damage Attributes")]
    public Transform[] damagePoints;
    public float attackRadius;
}
