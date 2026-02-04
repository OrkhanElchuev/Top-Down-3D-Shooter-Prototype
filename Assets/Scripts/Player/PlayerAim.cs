using System;
using NUnit.Framework.Constraints;
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

    private Vector2 mouseInput;
    private RaycastHit lastKnownRayHit;

    private void Start()
    {
        InitPlayer();
        AssignInputEvents();
    }

    private void Update()
    {
        AssignAimObject();
        UpdateAimVisuals();
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

    public Transform Aim() => aimObject;

    public RaycastHit GetMouseHitInfo()
    {
        // Create a ray that starts at the camera and goes through the mouse position.
        Ray ray = Camera.main.ScreenPointToRay(mouseInput);

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

    private void UpdateAimVisuals()
    {
        WeaponModel weaponModel = player.weaponVisuals.CurrentWeaponModel();

        weaponModel.transform.LookAt(aimObject);
        weaponModel.GunPoint.LookAt(aimObject);

        Transform gunPoint = player.weapon.GunPoint();
        Vector3 laserDirection = player.weapon.BulletDirection();

        float gunRange = player.weapon.CurrentWeapon().gunDistance;
        float laserTipLength = 0.5f;

        // Calculate the main endpoint of the laser beam.
        Vector3 endPoint = gunPoint.position + laserDirection * gunRange;

        // Make sure the laser doesn't penetrate the walls and other objects.
        if (Physics.Raycast(gunPoint.position, laserDirection, out RaycastHit hit, gunRange))
        {
            endPoint = hit.point;
            laserTipLength = 0f;
        }

        aimLaser.SetPosition(0, gunPoint.position);
        aimLaser.SetPosition(1, endPoint);
        // Extend the laser slightly further to create a visible tip effect.
        aimLaser.SetPosition(2, endPoint + laserDirection * laserTipLength);
    }

    private void AssignInputEvents()
    {
        if (player == null) return;

        controls = player.controls;

        // Aiming input events.
        controls.Character.Aim.performed += ctx => mouseInput = ctx.ReadValue<Vector2>();
        controls.Character.Aim.canceled += ctx => mouseInput = Vector2.zero;
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
