using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Reference to the weapon model in the world (visual + stats).
    [SerializeField] private WeaponModel weaponModel;

    private void OnTriggerEnter(Collider other)
    {
        // Try to get the player's weapon manager.
        PlayerWeaponManager weaponManager = other.GetComponent<PlayerWeaponManager>();

        // If the object entering the trigger is not the player, do nothing.
        if (weaponManager == null) return;

        // Give the player a new weapon created from this model.
        weaponManager.PickupWeapon(weaponModel);

        // Remove the pickup from the world.
        // Destroy(gameObject);
    }
}
