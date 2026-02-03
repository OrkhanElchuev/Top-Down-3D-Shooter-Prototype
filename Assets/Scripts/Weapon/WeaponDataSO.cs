using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapon System/Weapon Data")]
public class WeaponDataSO : ScriptableObject
{
    [Header("Weapon Type")]

    [Tooltip("Display name shown in UI.")]
    public string weaponName;
    public WeaponType weaponType;

    // ------------------------

    [Header("Shooting Setting")]

    [Tooltip("How the weapon fires: Auto = hold to keep firing, Single = one shot per press.")]
    public ShootType shootType;

    [Tooltip("Shots per second. Higher = faster firing.")]
    [Min(0.01f)] public float fireRate = 1f;

    [Tooltip("How many bullets are spawned per shot action.")]
    public int bulletsPerShot = 1;

    // ------------------------

    [Header("Ammo Settings")]

    [Tooltip("Starting bullets in the magazine when the weapon is created/equipped.")]
    [Min(0)] public int ammoInMagazine;

    [Tooltip("Starting reserve ammo carried outside the magazine.")]
    [Min(0)] public int totalReserveAmmo;

    [Tooltip("Maximum bullets the magazine can hold.")]
    [Min(1)] public int magazineCapacity;

    // ------------------------

    [Header("Spread Settings")]

    [Tooltip("Minimum spread amount when firing begins or after spread cooldown resets.")]
    [Min(0)] public float baseSpread;

    [Tooltip("Maximum spread the weapon can reach during continuous firing.")]
    [Min(0)] public float maxSpread;

    [Tooltip("How much spread increases per shot while firing continuously.")]
    [Min(0)] public float spreadIncreaseRate = 0.15f;

    // ------------------------

    [Header("Burst Settings")]

    [Tooltip("If true, this weapon supports burst mode (can be toggled on/off).")]
    public bool burstAvailable;

    [Tooltip("If true, the weapon starts in burst mode by default.")]
    public bool burstActive;

    [Tooltip("Projectiles fired per trigger action while burst mode is active.")]
    [Min(1)] public int burstBulletsPerShot;

    [Tooltip("Shots per second while burst mode is active (overrides fireRate).")]
    [Min(0.01f)] public float burstFireRate;

    [Tooltip("Delay in seconds between each projectile in the burst sequence.")]
    [Min(0f)] public float burstFireDelay = 0.1f;

    // ------------------------

    [Header("Weapon Settings")]
    
    [Tooltip("Time in seconds to reload the weapon.")]
    [Range(1, 3)]
    public float reloadSpeed = 1f;

    [Tooltip("How far the bullet travels.")]
    [Range(4, 10)]
    public float gunDistance = 4f;


    private void OnValidate()
    {
        // Prevent obviously invalid data combos.
        if (maxSpread < baseSpread) maxSpread = baseSpread;
        if (magazineCapacity < 1) magazineCapacity = 1;
        if (bulletsPerShot < 1) bulletsPerShot = 1;
        if (fireRate <= 0f) fireRate = 0.01f;
    }
}
