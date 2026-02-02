using UnityEngine;

public enum WeaponType
{
    Pistol,
    Revolver,
    Rifle,
    Shotgun,
    Sniper
}


[System.Serializable] // Make this class visible in the Unity Inspector.
public class Weapon 
{
    public WeaponType weaponType;

    public int ammoInMagazine;
    public int totalReserveAmmo;
    public int magazineCapacity;
    public float reloadTime = 1f;

    [Space]
    [Tooltip("Bullets per Second.")]
    public float fireRate = 1f; 
    private float lastShootTime;

    [HideInInspector] public WeaponModel weaponVisual;

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
