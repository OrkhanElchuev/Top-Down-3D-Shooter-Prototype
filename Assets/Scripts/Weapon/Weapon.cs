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
    public int ammo;
    public int maxAmmo;
}
