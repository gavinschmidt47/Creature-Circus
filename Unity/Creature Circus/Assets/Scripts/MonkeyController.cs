using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MonkeyController : MonoBehaviour
{
    public CharacterController controller;
    public PlayerController playerController;
    public float climbSpeed = 3.0f;
    public float jumpForce = 10.0f;

    private bool isClimbing = false;
    private Vector3 wallLocation;

    void Update()
    {
        if (isClimbing)
        {
            //Disables player movement
            playerController.enabled = false;

            //Climbs the wall
            Climb();

            //Checks for jump input
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Moves the character away from the wall
                StartCoroutine(JumpAway(transform.position - wallLocation));
            }
        }
        else
        {
            //Re-enables player movement
            playerController.enabled = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            //Sets the wall location and starts climbing
            wallLocation = other.transform.position;
            isClimbing = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            //Moves the character up from the wall
            StartCoroutine(JumpAway(Vector3.zero));
        }
    }

    //Called in update while climbing
    void Climb()
    {
        //Moves characterm up the wall
        controller.Move(Vector3.up * climbSpeed * Time.deltaTime);
    }

    IEnumerator JumpAway(Vector3 wallDirection)
    {
        //Need to neutralize sideways movement
        //Finds direction to move away from wall
        Vector3 jumpDirection = (wallDirection.normalized * jumpForce / 4) + transform.up;

        //Moves the character in a jump arc
        float jumpTime = 0.0f;
        while (jumpTime < 0.3f)
        {
            controller.Move(jumpDirection * jumpForce * Time.deltaTime);
            jumpTime += Time.deltaTime;
            yield return null;
        }

        //Resets climbing
        isClimbing = false;
    }
}

