using UnityEngine;

/// <summary>
/// Hold Components related to Player (for e.g. Player_Controls).
/// </summary>

public class Player : MonoBehaviour
{
    // REFERENCES
    public Player_Controls player_controls;

    private void Awake()
    {
        player_controls = new Player_Controls();
    }

    #region OnEnable / OnDisable

    private void OnEnable()
    {
        if (player_controls != null)
            player_controls.Enable();
    }

    private void OnDisable()
    {
        if (player_controls != null)
            player_controls.Disable();
    }

    #endregion
}
