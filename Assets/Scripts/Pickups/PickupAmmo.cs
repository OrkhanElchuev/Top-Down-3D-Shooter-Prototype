using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Determines which type of ammo box this pickup represents.
/// Used to select visuals and ammo distributions.
/// </summary>
public enum AmmoBoxType { smallBox, bigBox }

/// <summary>
/// Defines ammo ranges for a specific weapon type.
/// Used by ammo boxes to distribute bullets.
/// </summary>
[System.Serializable]
public struct AmmoData
{
    public WeaponType weaponType;
    [Range(10, 100)] public int minAmount;
    [Range(10, 100)] public int maxAmount;
}

/// <summary>
/// Ammo pickup interactable.
/// When collected, distributes random ammo amounts to the player's
/// currently owned weapons based on the box type.
/// </summary>
public class PickupAmmo : Interactable
{
    [Tooltip("Defines whether this ammo pickup is a small or big box.")]
    [SerializeField] private AmmoBoxType boxType;

    [Header("Ammo Distribution")]

    [Tooltip("Ammo ranges applied when this is a SMALL ammo box.")]
    [SerializeField] private List<AmmoData> smallBoxAmmo;

    [Tooltip("Ammo ranges applied when this is a BIG ammo box.")]
    [SerializeField] private List<AmmoData> bigBoxAmmo;

    [Header("Visual Models")]

    [Tooltip("Models used for each box type. Index must match AmmoBoxType enum.")]
    [SerializeField] private GameObject[] boxModel;

    private void Start()
    {
        // Enable correct visual model based on box type
        SetupBoxModel();
    }

    /// <summary>
    /// Called when the player interacts with this ammo box.
    /// Distributes ammo to all matching weapons and returns this object to the pool.
    /// </summary>
    public override void Interaction()
    {
        // Select ammo table based on box size
        List<AmmoData> currentAmmoList = smallBoxAmmo;

        if (boxType == AmmoBoxType.bigBox)
            currentAmmoList = bigBoxAmmo;
        
        // Loop through ammo definitions and add bullets to owned weapons
        foreach (AmmoData ammo in currentAmmoList)
        {
            Weapon weapon = weaponManager.WeaponInSlots(ammo.weaponType);

            AddBulletsToWeapon(weapon, GetBulletAmount(ammo));
        }

        // Return ammo box to object pool
        ObjectPooling.instance.ReturnObject(gameObject);
    }

    /// <summary>
    /// Returns a randomized bullet amount within the provided ammo range.
    /// </summary>
    private int GetBulletAmount(AmmoData ammoData)
    {
        // Ensure proper ordering in case values are reversed
        float min = Mathf.Min(ammoData.minAmount, ammoData.maxAmount);
        float max = Mathf.Max(ammoData.minAmount, ammoData.maxAmount);

        float randomAmmoAmount = Random.Range(min, max);

        return Mathf.RoundToInt(randomAmmoAmount);
    }

    /// <summary>
    /// Enables the correct ammo box model based on box type
    /// and updates highlight visuals.
    /// </summary>
    private void SetupBoxModel()
    {
        for (int i = 0; i < boxModel.Length; i++)
        {
            boxModel[i].SetActive(false);

            if (i == ((int)boxType))
            {
                boxModel[i].SetActive(true);
                // Pass MeshRenderer to base Interactable for highlight handling
                UpdateMeshAndMaterial(boxModel[i].GetComponent<MeshRenderer>());
            }
        }
    }

    /// <summary>
    /// Adds bullets to the weapon's reserve ammo.
    /// </summary>
    private void AddBulletsToWeapon(Weapon weapon, int amount)
    {
        // Player may not own this weapon type
        if (weapon == null) return;

        weapon.totalReserveAmmo += amount;
    }
}
