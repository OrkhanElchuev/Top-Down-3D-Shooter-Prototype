using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles weapon behavior for the player, including
/// bullet spawning, direction calculation, and firing animations.
/// </summary>

public class PlayerWeaponManager : MonoBehaviour
{
    // CONST VALUES
    // Name of the trigger parameter used by the firing animation.
    private const string FIRE = "Fire";
    // Default speed for bullet. To be used in a Mass Formula for a bullet to have dynamic impact.
    private const float REFERENCE_BULLET_SPEED = 20f;
    private const int MAX_WEAPON_SLOTS_ALLOWED = 2;

    // REFERENCES
    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private WeaponVisualManager visualManager;

    // BULLET
    [Header("Bullet Settings")]
    [Tooltip("Bullet prefab that will be instantiated when firing.")]
    [SerializeField] private GameObject bulletPrefab;

    [Tooltip("Location where bullet is fired from (at the tip of the weapon).")]
    [SerializeField] private Transform gunPoint;

    [Tooltip("Speed at which the bullet travels forward.")]
    [SerializeField] private float bulletSpeed;

    [Tooltip("Time to destroy the bullet object after firing.")]
    [SerializeField] private float bulletDestroyDelay = 5f;


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

    /// <summary>
    /// Builds a Weapon data object from a WeaponModel (Inspector values).
    /// Call this when a weapon is picked up.
    /// </summary>
    private Weapon CreateWeaponFromModel(WeaponModel model)
    {
        return new Weapon
        {
            weaponType = model.weaponType,
            magazineCapacity = model.magazineCapacity,

            // Clamp starting ammo so it can never exceed capacity.
            ammoInMagazine = Mathf.Min(model.startingAmmoInMagazine, model.magazineCapacity),

            totalReserveAmmo = model.startingReserveAmmo,
            reloadTime = model.reloadTime,

            // Link to the model so we can use its GunPoint.
            weaponVisual = model
        };
    }

    private void EquipStartingWeapon() => EquipWeapon(0);

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

    public void PickupWeapon(WeaponModel model)
    {
        // Check if Player has an empty slot to pickup another weapon.
        if (weaponSlots.Count >= MAX_WEAPON_SLOTS_ALLOWED) 
        {
            Debug.Log("No More Available Weapon Slots");
            return;
        }

        // Create weapon data from the picked weapon model.
        Weapon newWeapon = CreateWeaponFromModel(model);

        weaponSlots.Add(newWeapon);

        // Auto-equip the newly picked weapon.
        EquipWeapon(weaponSlots.Count - 1);
    }

    public void SetCurrentWeaponVisual(WeaponModel visual)
    {
        currentWeapon.weaponVisual = visual;
    }

    #endregion Public Methods

    #region Private Methods
    
    private void Shoot()
    {
        // Don't allow shooting while a reload is in progress
        if (isReloading) return;
        if (!currentWeapon.CanShoot()) return;

        if (currentWeapon.shootType == ShootType.Single)
            isShooting = false;

        Transform gunPoint = currentWeapon.weaponVisual.GunPoint;
        GameObject newBullet = ObjectPooling.instance.GetBullet();

        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        // Spawn a bullet at the gun point, rotated to face the same direction as the weapon.
        // GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        // Update the mass of the bullet depending on the speed of it and apply forward velocity.
        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
        rbNewBullet.linearVelocity = BulletDirection() * bulletSpeed;
        
        // Trigget Firing animation.
        GetComponentInChildren<Animator>().SetTrigger(FIRE);
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

    private void DropWeapon()
    {
        if (HasOnlyOneWeapon()) return;

        weaponSlots.Remove(currentWeapon);
        // Equip current weapon to the only weapon that's left.
        EquipWeapon(0);
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

    private System.Collections.IEnumerator ReloadRoutine()
    {
        isReloading = true;

        // Play reload VFX at the start
        PlayReloadVFX();

        // Wait based on the currently equipped weapon's reload time.
        yield return new WaitForSeconds(currentWeapon.reloadTime);

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
    }

    #endregion
}
