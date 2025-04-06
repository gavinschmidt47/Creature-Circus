using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheetahController : MonoBehaviour
{
    //PlayerStats
    [Header("Player Stats")]
    [Tooltip("Speed of the player")]
    public float playerSpeed;
    [Tooltip("Jump height of the player")]
    public float playerJump;
    [Tooltip("How long the player can hold the jump button")]
    public float heldJumpLength;
    [Tooltip("Maximum speed of the player")]
    public float maxSpeed;
    [Tooltip("Minimum speed to get rid of friction")]
    public float minSpeed;
    [Tooltip("How easily the player can stop moving")]
    public float breakForce;
    [Tooltip("Players attack box")]
    public GameObject attackBox;
    
    private Vector2 inVel;
    private bool doubleJump;
    private bool buttonHeld;
    private float boost;
    private float currSpeed;
    private bool grounded;
    internal bool paused;
    internal bool gameOver;


    //Input
    [Header("No Touchy <3")]
    public CheetahGameController gameController;
    public InputActionAsset inputs;
    
    private InputAction movement;


    //Player Components
    private Rigidbody rb;
    

    void OnEnable()
    {
        //Takes input from Move input action and activates listener
        movement = inputs.FindAction("Move");
        movement.Enable();
    }

    void Start()
    {
        //Gets CharacterController and rigidbody
        rb = gameObject.GetComponent<Rigidbody>();

        //Set speed boost
        boost = 1;

        //Set curr speed
        currSpeed = 0;
    }

    //Fixed update for physics regulation
    void FixedUpdate()
    {
        // Read input value
        inVel = movement.ReadValue<Vector2>();

        // Normalize input direction to ensure consistent movement speed
        Vector2 inputDirection = inVel.normalized;
        
        // Gradually increase speed to max speed or slow down to halt
        if (inVel.magnitude > 0)
        {
            //If force magnitude is less than player speed, set it to player speed
            if (currSpeed < playerSpeed)
            {
                currSpeed = playerSpeed;
            }
            //If speed is less than max speed, increase speed
            if (Mathf.Abs(rb.linearVelocity.z) < maxSpeed)
            {
            currSpeed += 200 * Time.deltaTime;
            }
            //If speed is greater than max speed, stop applying force
            else
            {
            currSpeed = 0;
            }
        }
        //Stop applying force if input is 0
        else
        {
            currSpeed = 0;
        }

        Vector2 targetVelocity = inputDirection * currSpeed * boost * Time.deltaTime;
        Vector3 appliedVelocity = new Vector3(targetVelocity.x, 0, targetVelocity.y);

        //Break force
        if (rb.linearVelocity.magnitude > 0 && inVel.magnitude == 0)
        {
            rb.AddForce(-rb.linearVelocity * breakForce, ForceMode.Force);
        }

        //Break inertia
        if (rb.linearVelocity.magnitude < minSpeed)
        {
            rb.AddForce(appliedVelocity * 3f, ForceMode.Impulse);
        }

        //Add force to rigidbody
        rb.AddForce(appliedVelocity, ForceMode.Force);
        // rb.linearVelocity = new Vector3(rb.linearVelocity.x, velocity.y, rb.linearVelocity.z);
        // Debug input values
        Debug.Log("Velocity: " + rb.linearVelocity);
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
        if (grounded)
        {
            rb.AddForce(Vector3.up * playerJump, ForceMode.Impulse);
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
        //Uses a timer to determine how long the player has been holding the jump button
        float timer = heldJumpLength;
        while (buttonHeld && timer > 0 && !grounded)
        {
            //Applies log momentum to the jump and small buff to speed
            rb.AddForce(Vector3.up * Mathf.Log(timer + 1) * playerJump * 0.5f, ForceMode.Force);
            boost = 1.5f;
            timer -= Time.deltaTime;
            yield return null;
        }
        boost = 1;
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
        if (other.gameObject.CompareTag("Win"))
        {
            gameController.WinGame();
        }
        if (other.gameObject.CompareTag("Lose"))
        {
            gameController.LoseGame();
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }

    //Called from PlayerInput
    public void Attack(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        StartCoroutine(inAttack(attackBox));
    }

    private IEnumerator inAttack(GameObject box)
    {
        box.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        box.SetActive(false);
    }
}
