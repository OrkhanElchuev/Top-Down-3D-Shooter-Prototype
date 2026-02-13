using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/// <summary>
/// Handles weapon behavior for the player, including
/// bullet spawning, direction calculation, and firing animations.
/// </summary>

public class PlayerWeaponManager : MonoBehaviour
{
    // CONST VALUES
    private const string FIRE = "Fire";
    private const float REFERENCE_BULLET_SPEED = 20f;
    private const int MAX_WEAPON_SLOTS_ALLOWED = 2;

    [Header("Starting Weapon")]
    [SerializeField] private WeaponDataSO weaponDataSO;

    [Header("Runtime")]
    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private WeaponVisualManager visualManager;
    
    [Header("Bullet Settings")]
    [Tooltip("Bullet prefab that will be instantiated when firing.")]
    [SerializeField] private GameObject bulletPrefab;

    [Tooltip("Location where bullet is fired from (at the tip of the weapon).")]
    [SerializeField] private Transform gunPoint;

    [Tooltip("Speed at which the bullet travels forward.")]
    [SerializeField] private float bulletSpeed;

    // AIM / VISUAL REFERENCES
    [Header("Weapon Settings")]
    [Tooltip("Transform that visually rotates the weapon toward the aim point.")]
    [SerializeField] private Transform weaponHolder;

    // INVENTORY SLOTS
    [Header("Inventory")]
    [Tooltip("Weapon Slots that can hold a single weapon per slot. Can be dynamically updated.")]
    [SerializeField] private List<Weapon> weaponSlots;

    // VFX
    [Header("Reload VFX")]
    [Tooltip("Visual effect played around the player when reloading")]
    [SerializeField] private GameObject reloadVFX;

    private bool isReloading;
    private bool isWeaponReady;
    private bool isShooting;

    private Player player;

    private void Start()
    {
        InitPlayer();
        InitInitialWeaponAmmo();
        AssignInputEvents();

        Invoke("EquipStartingWeapon", .1f);
    }

    private void Update()
    {
        if (isShooting)
            Shoot();
    }

    #region Initializations

    private void InitPlayer()
    {
        player = GetComponent<Player>();
        if (player == null)
            Debug.Log("Player Component is Missing!", this);
    }

    private void InitInitialWeaponAmmo()
    {
        // Determine how many bullets we can load into the magazine.
        // We cannot exceed the magazine capacity or the available reserve ammo.
        int load = Mathf.Min(currentWeapon.magazineCapacity, currentWeapon.totalReserveAmmo);

        // Fill the magazine with the calculated amount.
        currentWeapon.ammoInMagazine = load;

        // Remove the loaded bullets from the reserve ammo.
        currentWeapon.totalReserveAmmo -= load;
    }

    #endregion

    #region Public Methods

    public Transform GunPoint() => gunPoint;
    
    public Weapon CurrentWeapon() => currentWeapon;

    public void SetWeaponReady(bool ready) => isWeaponReady = ready;

    public bool WeaponReady() => isWeaponReady;

    public Vector3 BulletDirection()
    {
        Transform aim = player.aim.Aim();
        // Calculate direction from the gun to the aim point.
        Transform activeGunPoint = currentWeapon.weaponVisual.GunPoint;

        Vector3 direction = (aim.position - activeGunPoint.position).normalized;
        direction.y = 0;

        return direction;
    }

    /// <summary>
    /// Handles picking up a weapon from the world.
    /// Adds ammo if the weapon already exists, otherwise places it into a slot
    /// or replaces the currently equipped weapon if slots are full.
    /// </summary>
    /// <param name="newWeaponDataSO">Weapon data from the picked-up weapon.</param>
    public void PickupWeapon(Weapon newWeapon)
    {
        // If we already own this weapon type,
        // add the picked-up magazine ammo to the reserve and exit
        if (WeaponInSlots(newWeapon.weaponType) != null)
        {
            WeaponInSlots(newWeapon.weaponType).totalReserveAmmo += newWeapon.ammoInMagazine;
            return;
        }

        // If weapon slots are full AND the picked weapon is different
        // from the currently equipped weapon, replace the current weapon
        if (weaponSlots.Count >= MAX_WEAPON_SLOTS_ALLOWED && newWeapon.weaponType != currentWeapon.weaponType) 
        {
            int weaponIndex = weaponSlots.IndexOf(currentWeapon);
            // Replace current weapon with the newly picked one
            weaponSlots[weaponIndex] = newWeapon;
            EquipWeapon(weaponIndex);
            return;
        }

        weaponSlots.Add(newWeapon);
        // Auto-equip the newly picked weapon
        EquipWeapon(weaponSlots.Count - 1);
    }

    public Weapon WeaponInSlots(WeaponType weaponType)
    {
        foreach (Weapon weapon in weaponSlots)
        {
            if (weapon.weaponType == weaponType)
                return weapon;
        }

        return null;
    }

    public void SetCurrentWeaponVisual(WeaponModel visual)
    {
        currentWeapon.weaponVisual = visual;
    }

    public bool HasOnlyOneWeapon() => weaponSlots.Count <= 1;

    public void EquipWeapon (int i)
    {
        if (i < 0 || i >= weaponSlots.Count) return;

        currentWeapon = weaponSlots[i];

        // Update the active weapon model in the player's hands
        if (visualManager != null)
        visualManager.RefreshVisuals();
    }

    #endregion Public Methods

    #region Private Methods
    
    private void Shoot()
    {
        // Don't allow shooting while a reload is in progress
        if (isReloading) return;
        if (!currentWeapon.CanShoot()) return;

        // Trigget Firing animation.
        GetComponentInChildren<Animator>().SetTrigger(FIRE);

        if (currentWeapon.shootType == ShootType.Single)
            isShooting = false;

        if (currentWeapon.BurstActivated())
        {
            StartCoroutine(BurstFire());                
            return; 
        }

        FireSingleBullet();
    }

    private void FireSingleBullet()
    {
        currentWeapon.ammoInMagazine--;

        Transform gunPoint = currentWeapon.weaponVisual.GunPoint;
        GameObject newBullet = ObjectPooling.instance.GetObject(bulletPrefab);

        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        // Spawn a bullet at the gun point, rotated to face the same direction as the weapon.
        // GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        Bullet bulleScript = newBullet.GetComponent<Bullet>();
        bulleScript.BulletSetup(currentWeapon.gunDistance, currentWeapon.bulletDamage);

        // Apply Spread effect to weapons.
        Vector3 bulletsDirection = currentWeapon.ApplyShootingSpread(BulletDirection());

        // Update the mass of the bullet depending on the speed of it and apply forward velocity.
        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
        rbNewBullet.linearVelocity = bulletsDirection * bulletSpeed;
    }

    private void EquipStartingWeapon()
    {
        weaponSlots[0] = new Weapon(weaponDataSO);
        EquipWeapon(0);
    }

    private void DropWeapon()
    {
        if (HasOnlyOneWeapon()) return;

        weaponSlots.Remove(currentWeapon);
        // Equip current weapon to the only weapon that's left.
        EquipWeapon(0);
    }

    private IEnumerator BurstFire()
    {
        SetWeaponReady(false);

        for (int i = 0; i < currentWeapon.bulletsPerShot; i++)
        {
            if (!currentWeapon.HaveEnoughAmmo()) break;
            FireSingleBullet();
            yield return new WaitForSeconds(currentWeapon.burstFireDelay);

            if (i >= currentWeapon.bulletsPerShot)
                SetWeaponReady(true);
        }
    }

    /// <summary>
    /// Spawns a reload visual effect around the player.
    /// </summary>
    private void PlayReloadVFX()
    {
        if (reloadVFX == null) return;

        // Position the effect relative to the player.
        Vector3 spawnPosition = transform.position;

        // Spawn the VFX with no rotation.
        GameObject vfx = Instantiate(reloadVFX, spawnPosition, Quaternion.identity);

        // Parent it to the player so it follows during reload
        vfx.transform.SetParent(transform);

        // Destroy after the particle finishes.
        Destroy(vfx, 2f);
    }

    private void TryReload()
    {
        // Prevent reload spam
        if (isReloading) return;
        // Only reload if it is actually allowed.
        if (!currentWeapon.CanReload()) return;

        SetWeaponReady(false);

        // Start reload timing (per weapon)
        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        isReloading = true;

        // Play reload VFX at the start
        PlayReloadVFX();

        // Wait based on the currently equipped weapon's reload time.
        yield return new WaitForSeconds(currentWeapon.reloadSpeed);

        // Refill ammo after the delay.
        currentWeapon.ReloadAmmo();

        SetWeaponReady(true);
        isReloading = false;
    }

    private void AssignInputEvents()
    {
        PlayerControls controls = player.controls;

        controls.Character.Fire.performed += ctx => isShooting = true;
        controls.Character.Fire.canceled += ctx => isShooting = false;

        controls.Character.EquipSlot1.performed += ctx => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += ctx => EquipWeapon(1);
        controls.Character.DropCurrentWeapon.performed += ctx => DropWeapon();
        
        controls.Character.Reload.performed += ctx => TryReload();

        controls.Character.ToggleWeaponBurst.performed += ctx => currentWeapon.ToggleBurst();
    }

    #endregion
}
