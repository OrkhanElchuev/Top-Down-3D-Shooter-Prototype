using System;
using UnityEngine;

/// <summary>
/// Player Movement Script which handles Player Movement and Running Input Actions.
/// Also handles player drop from height and constant downforce gravity on player.
/// </summary>

public class PlayerMovement : MonoBehaviour
{   
    // CONST VALUES
    private const string X_VELOCITY = "xVelocity";
    private const string Z_VELOCITY = "zVelocity";
    private const string IS_RUNNING = "isRunning";

    // REFERENCES
    private Player player;
    private PlayerControls player_controls; // Access to Input System.
    private CharacterController characterController; // Component on Player Prefab.
    private Camera mainCamera;
    private Animator animator;

    // GENERAL SETTINGS
    [Header("General Settings")]
    [SerializeField] private float gravityValue = 9.81f;

    // PLAYER MOVEMENT
    [Header("Movement Info")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float turnSpeed;

    private Vector3 movementDirection;
    private Vector2 moveInput;
    private float verticalVelocity; // Handles falling down.
    private float defaultGroundedVelocity = 0.5f; // On ground make sure to have a small downward pull.
    private float movementTransitionTime = 0.1f; // Assure smoother transition between animation types.
    private float speed;
    private bool isRunning;


    private void Start() 
    {
        InitPlayer();
        InitCharacterController();
        InitMainCamera();
        InitCharacterAnimation();
        InitLocalVariables();

        AssignInputEvents();

        // Stop the script if critical references are mising.
        if (characterController == null || mainCamera == null || animator == null)
        {
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        ApplyMovement();
        ApplyRotation();
        AnimatorControllers();
    }
    
    #region Initializations

    private void InitPlayer()
    {
        player = GetComponent<Player>();
        if (player == null)
            Debug.Log("Player Component is missing on Player!", this);
    }

    private void InitMainCamera()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
            Debug.Log("Main Camera not Found! It has to have the MainCamera tag.", this);
    }

    private void InitCharacterController()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
            Debug.Log("CharacterController component is missing on Player!", this);
    }

    private void InitCharacterAnimation()
    {
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
            Debug.Log("Animator component is missing on Child Object of Player", this);
    }

    private void InitLocalVariables()
    {
        speed = walkSpeed;
    }

    #endregion 

    #region Private Methods
    private void AssignInputEvents()
    {
        player_controls = player.controls;

        // Walking
        // Inside of Input System, if the Movement is performed, read context val and assign to moveInput.
        player_controls.Character.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        player_controls.Character.Movement.canceled += ctx => moveInput = Vector2.zero;

        // Running
        player_controls.Character.Run.performed += ctx =>
        {
            speed = runSpeed;
            isRunning = true;
        };

        player_controls.Character.Run.canceled += ctx =>
        {
            speed = walkSpeed;
            isRunning = false;
        };
    }

    private void AnimatorControllers()
    {
        // Convert movement direction to direction-only (ignore speed).
        Vector3 normalizedMovement = movementDirection.normalized;

        // How much is player moving left/right relative to where the player is facing.
        float xVelocity = Vector3.Dot(normalizedMovement, transform.right);
        // How much is player moving forward/back relative to where the player is facing.
        float zVelocity = Vector3.Dot(normalizedMovement, transform.forward);

        // String references are coming from the Animator Parameters (Blend Tree).
        animator.SetFloat(X_VELOCITY, xVelocity, movementTransitionTime, Time.deltaTime);
        animator.SetFloat(Z_VELOCITY, zVelocity, movementTransitionTime, Time.deltaTime);

        // Make sure the run animation is not ON when player stopped while holding SHIFT (Run).
        bool playRunAnimation = isRunning && movementDirection.magnitude > 0;
        animator.SetBool(IS_RUNNING, playRunAnimation);
    }

    private void ApplyRotation()
    {
        // Subtracting player position gives direction Vector.
        Vector3 lookingDirection = player.aim.GetMouseHitInfo().point - transform.position;
        lookingDirection.y = 0f; // Ignore vertical rotation.
        lookingDirection.Normalize(); // Keep direction, remove distance.

        // Create rotation that looks in the calculated direction.
        Quaternion targetRotation = Quaternion.LookRotation(lookingDirection);
        // Smoothly rotate from the current rotation toward the target rotation (Spherical Interpolation).
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime); 
    }

    private void ApplyMovement()
    {
        // The Z vector gets the Y value of moveInput to move Up and Down in 2D coordinates.
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        ApplyGravity();

        // Check the length of the vector, if > 0, then there is a value to apply.
        if (movementDirection.magnitude > 0)
        {
            characterController.Move(movementDirection * Time.deltaTime * speed);
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

    #endregion
}
