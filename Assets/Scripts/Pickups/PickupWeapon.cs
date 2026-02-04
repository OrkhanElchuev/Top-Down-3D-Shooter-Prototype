using UnityEngine;

public class PickupWeapon : Interactable
{
    private PlayerWeaponManager weaponManager;
    [SerializeField] private WeaponDataSO weaponDataSO;

    public override void Interaction()
    {
        weaponManager.PickupWeapon(weaponDataSO);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (weaponManager == null)
            weaponManager = other.GetComponent<PlayerWeaponManager>();
    }
}
