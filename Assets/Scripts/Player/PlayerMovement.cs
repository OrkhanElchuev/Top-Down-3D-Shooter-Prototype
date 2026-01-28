using System;
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
    private Vector2 aimInput;

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
