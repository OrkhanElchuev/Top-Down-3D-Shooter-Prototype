using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAim : MonoBehaviour
{
    // REFERENCES
    private Player player;
    private PlayerControls controls;

    // AIM
    [Header("Aim Info")]
    [Tooltip("A layer mask for shooting a Ray.")]

    [SerializeField] private LayerMask aimLayerMask;
    [Tooltip("A small visible cursor object to show the aiming position.")]

    [SerializeField] private Transform aimObject;
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
