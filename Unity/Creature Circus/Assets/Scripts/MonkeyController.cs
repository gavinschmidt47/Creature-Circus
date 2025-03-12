using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyController : MonoBehaviour
{
    public float open = 100f;
    public float range = 1f;
    public bool TouchingWall = false;
    public float UpwardSpeed = 3.3f;
   public GameObject monkey;

   float timePassed = 0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

          timePassed += Time.deltaTime;
        
        if(Input.GetKey("w") & TouchingWall == true)     
        {
            transform.position += Vector3.up * Time.deltaTime * UpwardSpeed;
            GetComponent<Rigidbody>().isKinematic = true;
        }

        if(Input.GetKeyUp("w"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }

        if(Input.GetKeyUp("space"))
        {
            GetComponent<PlayerController>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Climbable"))
        {
            TouchingWall = true;
            GetComponent<PlayerController>().enabled = false;
            Debug.Log("hit collision");
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Climbable"))
        {
            timePassed = 0f;
            TouchingWall = false;
            GetComponent<PlayerController>().enabled = true;
            Debug.Log("left collision");
        }
    }
}

