using System.Collections;
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
    public PlayerInteraction interaction { get; private set; }
    public PlayerHealth health { get; private set; }
    public Animator animator { get; private set; }


    private void Awake()
    {
        controls = new PlayerControls();
        aim = GetComponent<PlayerAim>();
        movement = GetComponent<PlayerMovement>();
        weapon = GetComponent<PlayerWeaponManager>();
        weaponVisuals = GetComponent<WeaponVisualManager>();
        interaction = GetComponent<PlayerInteraction>();
        health = GetComponent<PlayerHealth>();
        animator = GetComponentInChildren<Animator>();
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
