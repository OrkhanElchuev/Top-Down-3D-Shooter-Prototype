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



    public bool CanShoot()
    {
        return HaveEnoughAmmo();
    }

    public bool HaveEnoughAmmo()
    {
        if (ammoInMagazine > 0)
        {
            ammoInMagazine--;
            return true;
        }

        return false;
    }

    public bool CanReload()
    {
        if (ammoInMagazine == magazineCapacity) return false;

        if (totalReserveAmmo > 0) return true;
        
        return false;
    }

    public void ReloadAmmo()
    {
        // Return the leftover ammo into the total reserve ammo.
        totalReserveAmmo += ammoInMagazine;

        int ammoToReload = magazineCapacity;

        if (ammoToReload > totalReserveAmmo)
            ammoToReload = totalReserveAmmo;
        
        totalReserveAmmo -= ammoToReload;
        ammoInMagazine = ammoToReload;

        if (totalReserveAmmo < 0)
            totalReserveAmmo = 0;
    }
}
