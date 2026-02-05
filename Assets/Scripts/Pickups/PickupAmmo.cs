using System.Collections.Generic;
using UnityEngine;

public enum AmmoBoxType { smallBox, bigBox }
public class PickupAmmo : Interactable
{
    private PlayerWeaponManager weaponManager;

    [SerializeField] private AmmoBoxType boxType;
    
    [System.Serializable]
    public struct AmmoData
    {
        public WeaponType weaponType;
        public int amount;
    }

    [SerializeField] private List<AmmoData> smallBoxAmmo;
    [SerializeField] private List<AmmoData> bigBoxAmmo;

    [SerializeField] private GameObject[] boxModel;

    private void Start()
    {
        SetupBoxModel();
    }

    public override void Interaction()
    {
        List<AmmoData> currentAmmoList = smallBoxAmmo;

        if (boxType == AmmoBoxType.bigBox)
            currentAmmoList = bigBoxAmmo;
        
        foreach (AmmoData ammo in currentAmmoList)
        {
            Weapon weapon = weaponManager.WeaponInSlots(ammo.weaponType);

            AddBulletsToWeapon(weapon, ammo.amount);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (weaponManager == null)
            weaponManager = other.GetComponent<PlayerWeaponManager>();
    }

    private void SetupBoxModel()
    {
        for (int i = 0; i < boxModel.Length; i++)
        {
            boxModel[i].SetActive(false);

            if (i == ((int)boxType))
            {
                boxModel[i].SetActive(true);
                UpdateMeshAndMaterial(boxModel[i].GetComponent<MeshRenderer>());
            }
        }
    }

    private void AddBulletsToWeapon(Weapon weapon, int amount)
    {
        if (weapon == null) return;

        weapon.totalReserveAmmo += amount;
    }
}
