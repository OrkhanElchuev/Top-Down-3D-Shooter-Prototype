using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Manages switching between weapons, deactivating and activating required weapon.
/// </summary>

public class WeaponVisualManager : MonoBehaviour
{
    // REFERENCES
    [Header("Weapons List")]
    [SerializeField] private Transform[] weaponTransforms;

    [Header("Weapon Types")]
    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform rifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform sniper;

    private void Update()
    {
        ChooseWeaponBasedOnKeyInput();
    }

    private void ChooseWeaponBasedOnKeyInput()
    {
        // If "1, 2, 3, 4, 5" keys are pressed.
        if (Input.GetKeyDown(KeyCode.Alpha1)) ActivateThisWeapon(pistol);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) ActivateThisWeapon(revolver);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) ActivateThisWeapon(rifle);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) ActivateThisWeapon(shotgun);
        else if (Input.GetKeyDown(KeyCode.Alpha5)) ActivateThisWeapon(sniper);
    }

    private void ActivateThisWeapon(Transform weaponTransform)
    {
        if (weaponTransform == null)
        {
            Debug.Log("Tried to activate a weapon, but the reference is missing.", this);
            return;
        }

        DeactivateAllGuns();
        weaponTransform.gameObject.SetActive(true);        
    }

    private void DeactivateAllGuns()
    {
        if (weaponTransforms == null) return;

        for (int i = 0; i < weaponTransforms.Length; i++)
        {
            if (weaponTransforms[i] != null)
                weaponTransforms[i].gameObject.SetActive(false);
        }
    }
}
