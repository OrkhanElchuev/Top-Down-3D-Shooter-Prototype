using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapon System/Weapon Data")]
public class WeaponDataSO : ScriptableObject
{
    [Header("Weapon Type")]
    public string weaponName;
    public WeaponType weaponType;

    [Header("Shooting Setting")]
    public ShootType shootType;
    public float fireRate;
    public int bulletsPerShot = 1;

    [Header("Ammo Settings")]
    public int ammoInMagazine;
    public int totalReserveAmmo;
    public int magazineCapacity;

    [Header("Spread Settings")]
    public float baseSpread;
    public float maxSpread;
    public float spreadIncreaseRate = 0.15f;

    [Header("Burst Settings")]
    public bool burstAvailable;
    public bool burstActive;
    public int burstBulletsPerShot;
    public float burstFireRate;
    public float burstFireDelay = 0.1f;

    [Header("Weapon Settings")]
    [Range(1, 3)]
    public float reloadSpeed = 1f;
    [Range(4, 10)]
    public float gunDistance = 4f;
}
