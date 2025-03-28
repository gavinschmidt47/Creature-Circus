using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //PlayerStats
    [Header("Player Stats")]
    [Tooltip("Speed of the player")]
    public float playerSpeed;
    [Tooltip("Jump height of the player")]
    public float playerJump;
    [Tooltip("Amount of gravity applied to the player")]
    public float gravScale = -9.81f;
    [Tooltip("Multiplier for gravity")]
    public float gravMult;
    [Tooltip("How long the player can hold the jump button")]
    public float heldJumpLength;
    [Tooltip("Melee hitbox collider")]
    public GameObject attackBox;
    
    private float upVel;
    private Vector2 inVel;
    private Vector3 targetVel;
    private bool doubleJump;
    private bool buttonHeld;
    private float boost;
    internal bool invincible;
    internal bool infStam;
    internal bool paused;
    internal bool gameOver;
    

    //Input
    [Header("No Touchy <3")]
    public InputActionAsset inputs;
    
    private InputAction movement;
    private InputAction look;

// Animator
public Animator myAnimator;
    
    //Controller
    private CharacterController controller;
    private GameController gameController;

    void OnEnable()
    {
        //Takes input from Move input action and activates listener
        movement = inputs.FindAction("Move");
        movement.Enable();
        look = inputs.FindAction("Look");
        look.Enable();
    }

    void Start()
    {
        //Gets CharacterController which can use .SimpleMove
        controller = gameObject.GetComponent<CharacterController>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        //Set speed boost
        boost = 1;

        //Locks cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Set default values
        upVel = 0;
        inVel = Vector2.zero;
        targetVel = Vector3.zero;
        doubleJump = false;
        buttonHeld = false;
        invincible = false;
        infStam = false;
        paused = false;
        gameOver = false;

    }

    void Update()
    {
        //Look around
        Vector2 lookDir = look.ReadValue<Vector2>();
        transform.Rotate(0, lookDir.x, 0);

    }

    //Fixed update for physics regulation
    void FixedUpdate()
    {
        inVel = movement.ReadValue<Vector2>() * playerSpeed * Time.deltaTime * boost;
        //Apply gravity
        if (!controller.isGrounded)
            upVel += gravScale * gravMult * Time.deltaTime;

        //Convert to Vector3 for SimpleMove
        Vector3 move = transform.right * inVel.x + transform.forward * inVel.y;
        targetVel = new Vector3(move.x, upVel, move.z);

        //Apply input
        controller.Move(targetVel);

    // Check to see if moving, trigger movement animation
        if (move.x > 0.001f || move.z > 0.001f)
        {
            myAnimator.SetTrigger("Move");
            Debug.Log("Moving");
        }
        else
        {
            myAnimator.ResetTrigger("Move");
            Debug.Log("Not Moving");
        }

        if (upVel > 0.001f)
        {
            myAnimator.SetTrigger("Jump");
            Debug.Log("Jumping");
        }
        else
        {
            myAnimator.ResetTrigger("Jump");
            Debug.Log("Not Jumping");
        }
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
        //Uses a timer to determine how long the player has been holding the jump button
        float timer = heldJumpLength;
        while (buttonHeld && timer > 0 && !controller.isGrounded)
        {
            //Applies log momentum to the jump and small buff to speed
            upVel += Mathf.Log(timer + 1) * Time.deltaTime;
            boost = 1.5f;
            timer -= Time.deltaTime;
            yield return null;
        }
        boost = 1;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Gate1"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Rhino");
        }
        else if (hit.gameObject.CompareTag("Gate2"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Monkey");
        }
        else if (hit.gameObject.CompareTag("Gate3"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Cheetah");
        }
        else if (hit.gameObject.CompareTag("Win"))
        {
            gameController.WinGame();
        }
        else if (hit.gameObject.CompareTag("Enemy") && !invincible)
        {
            gameController.LoseGame();
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

    //Called from Unity Events
    public void InfStamina(bool inf)
    {
        Debug.Log("InfStamina method called with value: " + inf); // Debug log to confirm method call
        if (inf)
        {
            infStam = true;
        }
        else
        {
            infStam = false;
        }
        Debug.Log("Infinite Stamina set to: " + infStam);
    }
    public void Invincible(bool inv)
    {
        if (inv)
        {
            invincible = true;
        }
        else
        {
            invincible = false;
        }
    }
}
