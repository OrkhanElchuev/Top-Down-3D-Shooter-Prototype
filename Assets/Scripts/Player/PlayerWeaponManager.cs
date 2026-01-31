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

    [Tooltip("Transform representing the current aim position.")]
    [SerializeField] private Transform aim;

    // REFERENCES
    private Player player;

    
    private void Start()
    {
        InitPlayer();

        // Subscribe to fire input event.
        player.controls.Character.Fire.performed += ctx => Fire();
    }

    #region Initializations



    private void InitPlayer()
    {
        player = GetComponent<Player>();
        if (player == null)
            Debug.Log("Player Component is Missing!", this);
    }

    #endregion

    #region Public Methods

    public Transform GunPoint() => gunPoint;

    public Vector3 BulletDirection()
    {
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
        // Spawn a bullet at the gun point, rotated to face the same direction as the weapon.
        GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
        // Apply forward velocity to the bullet.
        newBullet.GetComponent<Rigidbody>().linearVelocity = BulletDirection() * bulletSpeed;
        Destroy(newBullet, bulletDestroyDelay); 

        // Trigget Firing animation.
        GetComponentInChildren<Animator>().SetTrigger(FIRE);
    }

    #endregion
}
