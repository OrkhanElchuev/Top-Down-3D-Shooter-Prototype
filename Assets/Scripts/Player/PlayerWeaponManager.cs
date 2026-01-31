using UnityEngine;

/// <summary>
/// Weapon Manager Class to handle shooting.
/// </summary>

public class PlayerWeaponManager : MonoBehaviour
{
    // CONST VALUES
    private const string FIRE = "Fire";

    // BULLET
    [Header("Bullet Settings")]
    [SerializeField] private GameObject bulletPrefab;

    [Tooltip("Location where bullet is fired from (at the tip of the weapon).")]
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float bulletSpeed;

    // REFERENCES
    private Player player;

    private void Start()
    {
        InitPlayer();

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

    #region Private Methods
    
    private void Fire()
    {
        // Spawn a bullet at the gun point, rotated to face the same direction as the weapon.
        GameObject newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
        // Apply forward velocity to the bullet.
        newBullet.GetComponent<Rigidbody>().linearVelocity = gunPoint.forward * bulletSpeed;
        Destroy(newBullet, 5); 

        // Trigget Firing animation.
        GetComponentInChildren<Animator>().SetTrigger(FIRE);
    }

    #endregion
}
