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
    private Animator animator;

    [Header("Weapon Types")]
    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform rifle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform sniper;

    private void Start()
    {
        ActivateThisWeapon(pistol);

        InitAnimator();
    }

    private void Update()
    {
        ChooseWeaponBasedOnKeyInput();
    }

    #region Initializations

    private void InitAnimator()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.Log("Animator Component is missing on this Object", this);
    }

    #endregion

    #region Private Methods

    private void ChooseWeaponBasedOnKeyInput()
    {
        // If "1, 2, 3, 4, 5" keys are pressed.
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            ActivateThisWeapon(pistol);
            SwitchAnimationLayer(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateThisWeapon(revolver);
            SwitchAnimationLayer(1);
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActivateThisWeapon(rifle);
            SwitchAnimationLayer(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ActivateThisWeapon(shotgun);
            SwitchAnimationLayer(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ActivateThisWeapon(sniper);
            SwitchAnimationLayer(3);
        }
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

    // Animation layers have Indexes assigned 1 - Common Weapon, 2 Shotgun, 3 Sniper Rifle.
    private void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }

        animator.SetLayerWeight(layerIndex, 1);
    }

    #endregion
}
