using UnityEngine;

/// <summary>
/// Hold Components related to Player.
/// </summary>

public class Player : MonoBehaviour
{
    // REFERENCES
    public PlayerControls controls { get; private set; }
    public PlayerAim aim { get; private set; } // Read-only.

    private void Awake()
    {
        controls = new PlayerControls();
        aim = GetComponent<PlayerAim>();
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
