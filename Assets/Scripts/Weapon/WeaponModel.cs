using UnityEngine;

public class WeaponModel : MonoBehaviour
{
    [Header("Identity")]
    [Tooltip("What type of weapon this model represents.")]
    public WeaponType weaponType;

    [Header("Firing")]
    [Tooltip("Where bullets spawn from for this weapon model.")]
    [SerializeField] private Transform gunPoint;
    public Transform GunPoint => gunPoint;

    [Header("Ammo Settings")]
    [Tooltip("Maximum bullets the magazine can hold.")]
    [Min(1)] public int magazineCapacity = 12;

    [Tooltip("Ammo the weapon starts with in the magazine when first picked up.")]
    [Min(0)] public int startingAmmoInMagazine = 12;

    [Tooltip("Ammo the weapon starts with in reserve when first picked up.")]
    [Min(0)] public int startingReserveAmmo = 36;

    [Header("Reload Settings")]
    [Tooltip("How long a reload takes (seconds). Smaller = faster reload.")]
    [Min(0f)] public float reloadTime = 1.2f;
}
