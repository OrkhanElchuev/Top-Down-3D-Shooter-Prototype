using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handle Aiming with the mouse and adding a cursor object.
/// </summary>

public class PlayerAim : MonoBehaviour
{
    // REFERENCES
    private Player player;
    private PlayerControls controls;

    // AIM
    [Header("Aim Settings")]
    [Tooltip("A layer mask for shooting a Ray.")]
    [SerializeField] private LayerMask aimLayerMask;

    [Tooltip("A small visible cursor object to show the aiming position.")]
    [SerializeField] private Transform aimObject;

    [Header("Aim Visual - Laser")]
    [Tooltip("Laser pointer coming from the weapon, to help with aiming.")]
    [SerializeField] private LineRenderer aimLaser;

    [SerializeField] private float laserTipLength = 0.5f;

    private Vector2 aimInput;
    private RaycastHit lastKnownRayHit;

    private void Start()
    {
        InitPlayer();
        AssignInputEvents();
    }

    private void Update()
    {
        AssignAimObject();
        UpdateAimLaser();
    }

    #region Initializations

    private void InitPlayer()
    {
        player = GetComponent<Player>();
        if (player == null)
            Debug.Log("Player Component is not Assigned on this Object", this);
    }

    #endregion

    #region Public Methods

    public RaycastHit GetMouseHitInfo()
    {
        // Create a ray that starts at the camera and goes through the mouse position.
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        // Cast the ray into the world and check if it hits something on the aimLayerMask.
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lastKnownRayHit = hitInfo;
            // Return the world position of the hit.
            return hitInfo; 
        }
        return lastKnownRayHit;
    }

    #endregion

    #region Private Methods

    private void UpdateAimLaser()
    {
        Transform gunPoint = player.weapon.GunPoint();
        Vector3 laserDirection = player.weapon.BulletDirection();
        float gunRange = 4f; // Temporary distance.

        Vector3 endPoint = gunPoint.position + laserDirection * gunRange;

        aimLaser.SetPosition(0, gunPoint.position);
        aimLaser.SetPosition(1, endPoint);
        aimLaser.SetPosition(2, endPoint + laserDirection * laserTipLength);
    }

    private void AssignInputEvents()
    {
        if (player == null) return;

        controls = player.controls;

        // Aiming
        controls.Character.Aim.performed += ctx => aimInput = ctx.ReadValue<Vector2>();
        controls.Character.Aim.canceled += ctx => aimInput = Vector2.zero;
    }

    private void AssignAimObject()
    {
        if (aimObject == null) return;

        // Get the aim point once (one raycast per frame).
        Vector3 aimPoint = GetMouseHitInfo().point;

        // Place the aim object at the hit XZ and clamp Y to a fixed height next to the player.
        aimObject.position = new Vector3(aimPoint.x, transform.position.y + 1f, aimPoint.z);
    }

    #endregion
}
