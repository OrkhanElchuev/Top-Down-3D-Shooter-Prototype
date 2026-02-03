using UnityEngine;

public enum WeaponType { Pistol, Revolver, Rifle, Shotgun, Sniper }
public enum ShootType { Auto, Single }


[System.Serializable] // Make this class visible in the Unity Inspector.
public class Weapon 
{
    [Header("Weapon Settings")]
    public WeaponType weaponType;
    public ShootType shootType;

    public bool burstAvailable;

    // Burst runtime state
    private bool burstActive;
    public float burstFireDelay { get; private set; }
    private int burstBulletsPerShot;
    private float burstFireRate;

    [Header("Ammo Settings")]
    public int ammoInMagazine;
    public int totalReserveAmmo;
    public int magazineCapacity;
    public float reloadSpeed = 1f;

    // SHOOTING SETTINGS
    public int bulletsPerShot { get; private set; }

    private float fireRate = 1f; 
    private float defaultFireRate;
    private float lastShootTime;

    public float gunDistance { get; private set; }

    // SPREAD SETTINGS
    private float baseSpread = 1f;
    private float currentSpread = 2f;
    private float maxSpread = 3f;
    private float spreadIncreaseRate = 0.15f;
    private float spreadCooldown = 1f;
    private float lastSpreadUpdateTime;

    [HideInInspector] public WeaponModel weaponVisual;

    public Weapon(WeaponDataSO weaponDataSO)
    {
        // WEAPON TYPE SETTINGS
        weaponType = weaponDataSO.weaponType;

        // SHOOT SETTINGS
        fireRate = weaponDataSO.fireRate;
        shootType = weaponDataSO.shootType;
        bulletsPerShot = weaponDataSO.bulletsPerShot;

        // SPREAD SETTING
        baseSpread = weaponDataSO.baseSpread;
        maxSpread = weaponDataSO.maxSpread;
        spreadIncreaseRate = weaponDataSO.spreadIncreaseRate;

        // WEAPON SETTINGS
        reloadSpeed = weaponDataSO.reloadSpeed;
        gunDistance = weaponDataSO.gunDistance;

        // AMMO SETTINGS
        ammoInMagazine = weaponDataSO.ammoInMagazine;
        totalReserveAmmo = weaponDataSO.totalReserveAmmo;
        magazineCapacity = weaponDataSO.magazineCapacity;
        
        // BURST SETTINGS
        burstAvailable = weaponDataSO.burstAvailable;
        burstActive = weaponDataSO.burstActive;
        burstBulletsPerShot = weaponDataSO.burstBulletsPerShot;
        burstFireRate = weaponDataSO.burstFireRate;
        burstFireDelay = weaponDataSO.burstFireDelay;

        defaultFireRate = fireRate;
    }

    public Vector3 ApplyShootingSpread(Vector3 originalDirection)
    {
        // Update the current spread value based on firing timing.
        UpdateSpread();

        // Use separate yaw/pitch so distribution isn't biased.
        float yaw = Random.Range(-currentSpread, currentSpread);
        float pitch = Random.Range(-currentSpread, currentSpread);

        Quaternion spreadRotation = Quaternion.Euler(pitch, yaw, 0f);
        return spreadRotation * originalDirection;
    }

    // Increases the current spread while clamping it
    // so it never goes below baseSpread or above maxSpread.
    private void IncreaseSpread()
    {
        currentSpread = Mathf.Clamp(currentSpread + spreadIncreaseRate, baseSpread, maxSpread);
    }

    // Handles whether spread should reset or continue increasing.
    private void UpdateSpread()
    {
        // If enough time has passed since the last shot,
        // reset spread back to the base value.
        if (Time.time > lastSpreadUpdateTime + spreadCooldown)
            currentSpread = baseSpread;
        else
            IncreaseSpread();

        lastSpreadUpdateTime = Time.time;
    }

    #region Burst Mode Methods

    public bool BurstActivated()
    {
        if (weaponType == WeaponType.Shotgun)
        {
            burstFireDelay = 0;
            return true;
        }

        return burstActive;
    }

    public void ToggleBurst()
    {
        if (burstAvailable == false)
            return;

        burstActive = !burstActive;

        if (burstActive)
        {
            bulletsPerShot = burstBulletsPerShot;
            fireRate = burstFireRate;
        }
        else
        {
            bulletsPerShot = 1;
            fireRate = defaultFireRate;
        }
    }

    #endregion

    public bool CanShoot() => HaveEnoughAmmo() && ReadyToFire();

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
        // Protect against fireRate <= 0
        float interval = 1f / Mathf.Max(0.0001f, fireRate);

        // Interval between each bullet = Fire rate.
        if (Time.time > lastShootTime + interval)
        {
            lastShootTime = Time.time;
            return true;
        }

        return false;
    }
}
