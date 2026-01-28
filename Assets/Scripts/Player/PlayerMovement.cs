using System;
using UnityEditor.EditorTools;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    // REFERENCES
    private Player_Controls player_controls; // Access to Input System.
    private CharacterController characterController; // Component on Player Prefab.

    // PLAYER MOVEMENT
    [Header("Movement Info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravityValue = 9.81f;
    private Vector3 movementDirection;
    private Vector2 moveInput;
    private float verticalVelocity; // Handles falling down.
    private float defaultGroundedVelocity = 0.5f; // On ground make sure to have a small downward pull.

    // AIM
    [Header("Aim Info")]
    [Tooltip("A layer mask for shooting a Ray.")]
    [SerializeField] private LayerMask aimLayerMask;
    [Tooltip("A small visible cursor object to show the aiming position.")]
    [SerializeField] private Transform aimPoint;
    private Vector2 aimInput;
    private Vector3 lookingDirection;


    private void Awake()
    {
        player_controls = new Player_Controls();

        // Inside of Input System, if the Movement is performed, read context val and assign to moveInput.
        player_controls.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        // If movement action is stopped, assign 0 to moveInput.
        player_controls.Character.Movement.canceled += context => moveInput = Vector2.zero;

        player_controls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        player_controls.Character.Aim.canceled += context => aimInput = Vector2.zero;
    }

    private void Start() 
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        ApplyMovement();
        AimTowardsMousePos();
    }

    private void AimTowardsMousePos()
    {
        // Create a ray starting from the camera and going through the mouse position.
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        // Check if ray hits any objects defined in the layer mask.
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            // hitinfo.point = world position where the ray hits
            // subtracting player position gives direction Vector.
            lookingDirection = hitInfo.point - transform.position;
            lookingDirection.y = 0f; // Ignore vertical rotation.
            lookingDirection.Normalize(); // Keep direction, remove distance.

            transform.forward = lookingDirection; 
            // A visual crosshair to show the Hit position.
            aimPoint.position = new Vector3 (hitInfo.point.x, transform.position.y, hitInfo.point.z);
        }
    }

    private void ApplyMovement()
    {
        // The Z vector gets the Y value of moveInput to move Up and Down in 2D coordinates.
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        ApplyGravity();

        // Check the length of the vector, if > 0, then there is a value to apply.
        if (movementDirection.magnitude > 0)
        {
            characterController.Move(movementDirection * Time.deltaTime * moveSpeed);
        }
    }


    private void ApplyGravity()
    {
        // Pull the player down from Air (for e.g. when jumping from platform).
        if (!characterController.isGrounded)
        {
            verticalVelocity -= gravityValue * Time.deltaTime;
            movementDirection.y = verticalVelocity;
        }
        else
            // Apply downward force when on ground.
            verticalVelocity = -defaultGroundedVelocity;
    }

    private void OnEnable() 
    {
        player_controls.Enable();    
    }

    private void Oisable()
    {
        player_controls.Disable();        
    }
}
