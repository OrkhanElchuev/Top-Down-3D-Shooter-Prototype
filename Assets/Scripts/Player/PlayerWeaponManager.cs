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


    // REFERENCES
    [SerializeField] private Weapon currentWeapon;
    private Player player;

    private void Start()
    {
        InitPlayer();
        InitInitialWeaponAmmo();
        AssignInputEvents();
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
        currentWeapon.ammo = currentWeapon.maxAmmo;
    }

    #endregion

    #region Public Methods

    public Transform GunPoint() => gunPoint;

    public Vector3 BulletDirection()
    {
        Transform aim = player.aim.Aim();
        // Calculate direction from the gun to the aim point.
        Vector3 direction = (aim.position - gunPoint.position).normalized;
        direction.y = 0;

        // Rotate weapon holder toward the aim position.
        weaponHolder.LookAt(aim);
        gunPoint.LookAt(aim);

        return direction;
    }

    #endregion Public Methods

    #region Private Methods
    
    private void Fire()
    {
        if (currentWeapon.ammo <= 0)
        {
            Debug.Log("Out of ammo.");
            return;
        } 

        currentWeapon.ammo--;

        // Spawn a bullet at the gun point, rotated to face the same direction as the weapon.
        GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        // Update the mass of the bullet depending on the speed of it and apply forward velocity.
        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
        rbNewBullet.linearVelocity = BulletDirection() * bulletSpeed;
        
        Destroy(newBullet, bulletDestroyDelay); 
        // Trigget Firing animation.
        GetComponentInChildren<Animator>().SetTrigger(FIRE);
    }

    private void EquipWeapon (int i)
    {
        currentWeapon = weaponSlots[i];
    }

    private void AssignInputEvents()
    {
        PlayerControls controls = player.controls;

        // Subscribe to fire input event.
        controls.Character.Fire.performed += ctx => Fire();

        controls.Character.EquipSlot1.performed += ctx => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += ctx => EquipWeapon(1);
    }

    #endregion
}
