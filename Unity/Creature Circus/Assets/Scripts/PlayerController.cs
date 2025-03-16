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
    public GameObject attackBox;
    
    private float upVel;
    private Vector2 inVel;
    private Vector3 targetVel;
    private bool doubleJump;
    private bool buttonHeld;
    private float boost;


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

        //Set speed boost
        boost = 1;
    }

    //Fixed update for physics regulation
    void FixedUpdate()
    {
        //Gets input and sets correct magnitude
        inVel = movement.ReadValue<Vector2>() * playerSpeed * Time.deltaTime * boost;

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

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Gate1"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Rhino");
        }
        else if (other.gameObject.CompareTag("Gate2"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Monkey");
        }
        else if (other.gameObject.CompareTag("Gate3"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Cheetah");
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
