using UnityEngine;

/// <summary>
/// Weapon Manager Class to handle shooting.
/// </summary>

public class PlayerWeaponManager : MonoBehaviour
{
    // CONST VALUES
    private const string FIRE = "Fire";

    // REFERENCES
    private Player player;

    # region Awake / Start / Update

    private void Start()
    {
        InitPlayer();

        player.player_controls.Character.Fire.performed += ctx => Fire();
    }

    #endregion

    #region Initializations

    private void InitPlayer()
    {
        player = GetComponent<Player>();
        if (player == null)
            Debug.Log("Player Component is Missing!", this);
    }

    #endregion

    #region Private Methods
    
    private void Fire()
    {
        GetComponentInChildren<Animator>().SetTrigger(FIRE);
    }

    #endregion
}
