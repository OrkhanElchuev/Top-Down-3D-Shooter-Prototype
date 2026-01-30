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
        // If "1" key is pressed.
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ActivateThisWeapon(pistol);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            ActivateThisWeapon(revolver);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            ActivateThisWeapon(rifle);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            ActivateThisWeapon(shotgun);    

        if (Input.GetKeyDown(KeyCode.Alpha5))
            ActivateThisWeapon(sniper);
    }

    private void ActivateThisWeapon(Transform weaponTransform)
    {
        DeactivateAllGuns();
        weaponTransform.gameObject.SetActive(true);        
    }

    private void DeactivateAllGuns()
    {
        for (int i = 0; i < weaponTransforms.Length; i++)
        {
            weaponTransforms[i].gameObject.SetActive(false);
        }
    }
}
