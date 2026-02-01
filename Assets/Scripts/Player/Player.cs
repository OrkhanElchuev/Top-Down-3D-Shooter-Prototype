using UnityEngine;

/// <summary>
/// Hold Components related to Player.
/// </summary>

public class Player : MonoBehaviour
{
    // REFERENCES
    public PlayerControls controls { get; private set; }
    public PlayerAim aim { get; private set; }
    public PlayerMovement movement { get; private set; }
    public PlayerWeaponManager weapon { get; private set; }
    public WeaponVisualManager weaponVisuals { get; private set; }


    private void Awake()
    {
        controls = new PlayerControls();
        aim = GetComponent<PlayerAim>();
        movement = GetComponent<PlayerMovement>();
        weapon = GetComponent<PlayerWeaponManager>();
        weaponVisuals = GetComponent<WeaponVisualManager>();
    }

    #region OnEnable / OnDisable

    private void OnEnable()
    {
        if (controls != null)
            controls.Enable();
    }

    private void OnDisable()
    {
        if (controls != null)
            controls.Disable();
    }

    #endregion
}
