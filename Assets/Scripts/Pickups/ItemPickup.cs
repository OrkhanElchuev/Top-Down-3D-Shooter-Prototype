using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // Reference to the weapon model in the world (visual + stats).
    [SerializeField] private WeaponDataSO weaponDataSO;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerWeaponManager>()?.PickupWeapon(weaponDataSO);
    }
}
