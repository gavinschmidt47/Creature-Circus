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
    [Tooltip("Manage power of the held jump")]
    public float heldJumpPower;
    [Tooltip("Melee hitbox collider")]
    public GameObject attackBox;
    [Tooltip("Audio source for the hit sound")]
    public AudioSource hitSound;
    [Tooltip("Audio source for the jump sound")]
    public AudioSource jumpSound;
    [Tooltip("Bounce multiplier for the jump pad")]
    public float bounceMultiplier = 1.5f;
    
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
    [Header("Animator")]
    [Tooltip("Animator for the player")]
    public Animator myAnimator;

    
    //Controller
    private CharacterController controller;
    private GameController gameController;

    //UI
    [Header("UI")]
    [Tooltip("UI for cheats")]
    public GameObject infStaminaUI;
    public GameObject invincibleUI;

    void OnEnable()
    {
        //Takes input from Move input action and activates listener
        movement = inputs.FindAction("Move");
        movement.Enable();
        look = inputs.FindAction("Look");
        look.Enable();
    }

    void OnDisable()
    {
        //Disables input when not in use
        movement.Disable();
        look.Disable();
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

        //Lock rotations by overriding transform rotation
        transform.rotation = Quaternion.identity;
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
        if ((inVel.x > 0.001f || inVel.x < -0.001f) || (inVel.y > 0.001f || inVel.y < -0.001f))
        {
            myAnimator.SetBool("Walking", true);
        }
        else
        {
            myAnimator.SetBool("Walking", false);
        }
        //Apply gravity
        if (!controller.isGrounded && !doubleJump)
        {
            upVel += gravScale * gravMult * Time.deltaTime;
        }
        else if (!controller.isGrounded && doubleJump)
        {
            upVel += heldJumpPower * Time.deltaTime;
        }
        //Convert to Vector3 for SimpleMove
        Vector3 move = transform.right * inVel.x + transform.forward * inVel.y;
        targetVel = new Vector3(move.x, upVel, move.z);

        //Apply input
        controller.Move(targetVel);

        // Check to see if moving, trigger movement animation
        
    }

    //Called from Player Input
    public void Jump(InputAction.CallbackContext context)
    {
        //Checks if button is being held
        if (context.started)
        {
            buttonHeld = true;
            //Play sound
            jumpSound.PlayOneShot(jumpSound.clip);
        }
        else if (context.canceled)
        {
            buttonHeld = false;
            doubleJump = false; 
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
        doubleJump = false;
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
            PlayerPrefs.SetInt("levelsCompleted", PlayerPrefs.GetInt("levelsCompleted") + 1);
            gameController.WinGame();
        }
        else if (hit.gameObject.CompareTag("Lose")
        {
            gameController.LoseGame();
        }
        else if (hit.gameObject.CompareTag("Enemy") && !invincible)
        {
            gameController.LoseGame();
        }
        else if (hit.gameObject.CompareTag("Jump"))
        {
            upVel = playerJump * bounceMultiplier;
            //Play sound
            jumpSound.PlayOneShot(jumpSound.clip);
        }
        
    }

    //Called from PlayerInput
    public void Attack(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        StartCoroutine(inAttack(attackBox));
        //Play sound
        hitSound.PlayOneShot(hitSound.clip);
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

            // Set the stamina UI to active
            infStaminaUI.SetActive(true);
        }
        else
        {
            infStam = false;

            // Set the stamina UI to inactive
            infStaminaUI.SetActive(false);
        }
        Debug.Log("Infinite Stamina set to: " + infStam);
    }
    public void Invincible(bool inv)
    {
        if (inv)
        {
            invincible = true;

            // Set the invincible UI to active
            invincibleUI.SetActive(true);
        }
        else
        {
            invincible = false;

            // Set the invincible UI to inactive
            invincibleUI.SetActive(false);
        }
    }
}
