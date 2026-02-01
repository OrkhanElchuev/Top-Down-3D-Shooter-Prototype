using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Manages switching between weapons, deactivating and activating required weapon.
/// </summary>

public class WeaponVisualManager : MonoBehaviour
{
    // REFERENCES
    [Header("References")]
    [SerializeField] private PlayerWeaponManager weaponManager;

    private Player player;

    [Header("Weapons List")]
    [SerializeField] private WeaponModel[] weaponModels;

    private void Awake()
    {
        // Cache the Player reference early so it's ready before Start/Update.
        player = GetComponent<Player>();

        // Auto-find weapon models under this object.
        weaponModels = GetComponentsInChildren<WeaponModel>(true);
    }

    private void Start()
    {
        ActivateCurrentWeaponVisuals();
    }

    private void Update()
    {
        ChooseWeaponBasedOnKeyInput();
    }

    #region Public Methods

    /// <summary>
    /// Returns the WeaponModel that matches the player's currently equipped weapon type.
    /// </summary>
    public WeaponModel CurrentWeaponModel()
    {
        if (player == null || player.weapon == null) return null;
        if (weaponModels == null || weaponModels.Length == 0) return null;

        // Get the weapon type of the player's currently equipped weapon.
        WeaponType weaponType = player.weapon.CurrentWeapon().weaponType;

        // Loop though all available weapon models.
        for (int i = 0; i < weaponModels.Length; i++)
        {   
            // If there is a match return it as a current weapon type.
            if (weaponModels[i] != null && weaponModels[i].weaponType == weaponType)
                return weaponModels[i];
        }

        return null;
    }

    public void RefreshVisuals()
    {
        ActivateCurrentWeaponVisuals();
    }

    #endregion

    #region Private Methods
        private void ChooseWeaponBasedOnKeyInput()
        {
            // If "1, 2, 3, 4, 5" keys are pressed.
            if (Input.GetKeyDown(KeyCode.Alpha1)) EquipSlotAndRefresh(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) EquipSlotAndRefresh(1);
            //else if (Input.GetKeyDown(KeyCode.Alpha3)) EquipSlotAndRefresh(2);
            //else if (Input.GetKeyDown(KeyCode.Alpha4)) EquipSlotAndRefresh(3);
            //else if (Input.GetKeyDown(KeyCode.Alpha5)) EquipSlotAndRefresh(4);
        }

    private void EquipSlotAndRefresh(int slotIndex)
    {
        // Equip the weapon slot.
        weaponManager.EquipWeapon(slotIndex);

        // After equipping, enable the correct visual.
        ActivateCurrentWeaponVisuals();
    }

    private void ActivateCurrentWeaponVisuals()
    {
        if (weaponModels == null || weaponModels.Length == 0)
        {
            Debug.Log("Tried to activate a weapon, but the reference is missing.", this);
            return;
        }

        DeactivateAllGuns();

        WeaponModel current = CurrentWeaponModel();
        if (current == null)
        {
            Debug.LogWarning("No WeaponModel matches the currently equipped WeaponType.", this);
            return;
        }

        // Enable only the current weapon model.
        current.gameObject.SetActive(true);

        // Inform the weapon manager which visual is active.
        weaponManager.SetCurrentWeaponVisual(current);      
    }

    private void DeactivateAllGuns()
    {
        for (int i = 0; i < weaponModels.Length; i++)
        {
            if (weaponModels[i] != null)
                weaponModels[i].gameObject.SetActive(false);
        }
    }
    #endregion
}
