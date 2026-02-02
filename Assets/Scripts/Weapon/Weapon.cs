using UnityEngine;

public enum WeaponType
{
    Pistol,
    Revolver,
    Rifle,
    Shotgun,
    Sniper
}

public enum ShootType
{
    Auto,
    Single
}


[System.Serializable] // Make this class visible in the Unity Inspector.
public class Weapon 
{
    [Header("Weapon Type")]
    public WeaponType weaponType;

    [Header("Ammo Settings")]
    public int ammoInMagazine;
    public int totalReserveAmmo;
    public int magazineCapacity;
    public float reloadTime = 1f;

    [Space]
    [Header("Shooting Settings")]
    [Tooltip("Bullets per Second.")]
    public float fireRate = 1f; 
    public ShootType shootType;
    private float lastShootTime;

    [Header("Recoil / Spread")]
    private float baseSpread = 1f;
    private float currentSpread = 2f;
    public float maxSpread = 3f;
    public float spreadIncreaseRate = 0.15f;

    private float lastSpreadUpdateTime;
    private float spreadCooldown = 0.5f;

    [HideInInspector] public WeaponModel weaponVisual;

    public Vector3 ApplyShootingSpread(Vector3 originalDirection)
    {
        // If shooting continues the spread increases too.
        UpdateSpread();
        float randomizedValue = Random.Range(-currentSpread, currentSpread);
        Quaternion spreadRotation = Quaternion.Euler(randomizedValue, randomizedValue, randomizedValue);
        
        return spreadRotation * originalDirection;
    }

    private void IncreaseSpread()
    {
        currentSpread = Mathf.Clamp(currentSpread + spreadIncreaseRate, baseSpread, maxSpread);
    }

    private void UpdateSpread()
    {
        if (Time.time > lastSpreadUpdateTime + spreadCooldown)
            currentSpread = baseSpread;
        else
            IncreaseSpread();

        lastSpreadUpdateTime = Time.time;
    }

    public bool CanShoot()
    {
        if (HaveEnoughAmmo() && ReadyToFire())
        {
            ammoInMagazine--;
            return true;
        }
        return false;
    }

    public bool HaveEnoughAmmo()
    {
        if (ammoInMagazine > 0)
            return true;

        return false;
    }

    public bool CanReload()
    {
        if (ammoInMagazine == magazineCapacity) return false;
        return totalReserveAmmo > 0; 
    }

    public void ReloadAmmo()
    {
        // Return the leftover ammo into the total reserve ammo.
        totalReserveAmmo += ammoInMagazine;

        int ammoToReload = Mathf.Min(magazineCapacity, totalReserveAmmo);
        
        totalReserveAmmo -= ammoToReload;
        ammoInMagazine = ammoToReload;
    }

    private bool ReadyToFire()
    {
        // Interval between each bullet = Fire rate.
        if (Time.time > lastShootTime + 1 / fireRate)
        {
            lastShootTime = Time.time;
            return true;
        }

        return false;
    }
}
