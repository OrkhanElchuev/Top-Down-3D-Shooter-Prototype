using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    private Player_Controls player_controls;
    private CharacterController characterController;

    [Header("Movement Info")]
    [SerializeField] private float moveSpeed;

    private Vector3 movementDirection;
    private Vector2 moveInput;
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
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);

        // Check the length of the vector, if > 0, then there is a value to apply.
        if (movementDirection.magnitude > 0)
        {
            characterController.Move(movementDirection * Time.deltaTime * moveSpeed);
        }
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
