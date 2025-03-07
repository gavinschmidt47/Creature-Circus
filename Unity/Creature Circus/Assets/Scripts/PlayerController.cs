using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //PlayerStats
    public float playerSpeed;
    public float playerJump;
    public float gravScale = -9.81f;
    public float gravMult;
    public float heldJumpLength;

    private float upVel;
    private Vector2 inVel;
    private Vector3 targetVel;
    private bool doubleJump;
    private bool buttonHeld;


    //Input
    public InputActionAsset inputs;
    
    private InputAction movement;

    
    //Controller
    private CharacterController controller;

    void OnEnable()
    {
        //Takes input from Move input action and activates listener
        movement = inputs.FindAction("Move");
        movement.Enable();
    }

    void Start()
    {
        //Gets CharacterController which can use .SimpleMove
        controller = gameObject.GetComponent<CharacterController>();
    }

    //Fixed update for physics regulation
    void FixedUpdate()
    {
        //Gets input and sets correct magnitude
        inVel = movement.ReadValue<Vector2>() * playerSpeed * Time.deltaTime;

        // Debug the state of isGrounded
        Debug.Log("Is Grounded: " + controller.isGrounded);

        //Apply gravity
        if (!controller.isGrounded)
            upVel += gravScale * gravMult * Time.deltaTime;

        //Convert to Vector3 for SimpleMove
        targetVel = new Vector3(inVel.x, upVel, inVel.y);

        //Apply input
        controller.Move(targetVel);
    }

    //Called from Player Input
    public void Jump(InputAction.CallbackContext context)
    {
        //Checks if button is being held
        if (context.started)
        {
            buttonHeld = true;
        }
        else if (context.canceled)
        {
            buttonHeld = false;
        }

        //Gets rid of 2/3 button contexts
        if (!context.started) return;

        //Checks if controller is on the ground or if its used double jump
        if (controller.isGrounded)
        {
            upVel = playerJump;
            doubleJump = false; // Reset double jump when grounded
        }
        else if (!doubleJump)
        {
            //Apply long jump
            StartCoroutine(HeldJump());
            doubleJump = true;
        }
    }

    private IEnumerator HeldJump()
    {
        float timer = heldJumpLength;
        while (buttonHeld && timer > 0 && !controller.isGrounded)
        {
            upVel += Mathf.Log(timer + 1) * Time.deltaTime;
            timer -= Time.deltaTime;
            yield return null;
        }
    }
}
