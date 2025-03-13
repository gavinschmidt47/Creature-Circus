using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RhinoController : MonoBehaviour
{
    //UI
    public Slider chargeBar;


    //Settings
    public float chargeSpeed = 10.0f;
    public float chargeTime = 2.0f;
    public float chargeCooldown = 5.0f;

    private bool charging = false;
    private float chargingTime = 0.0f;
    private float coolingTime;
    private bool inCooling = false;
    private Image chargeColor;


    //Controller
    private PlayerController playerController;
    private CharacterController controller;

    void Start()
    {
        //Gets CharacterController which can use .SimpleMove
        controller = gameObject.GetComponent<CharacterController>();

        //Gets the player controller
        playerController = gameObject.GetComponent<PlayerController>();

        //Sets the UI
        chargeBar.maxValue = chargeCooldown;
        coolingTime = chargeCooldown;
        chargeBar.value = coolingTime;
        chargeColor = chargeBar.fillRect.GetComponent<Image>();
    }

    void Update()
    {
        //Cooldown
        if (coolingTime < chargeCooldown)
        {
            coolingTime += Time.deltaTime;
            inCooling = true;
        }
        else
        {
            inCooling = false;
            coolingTime = chargeCooldown;
            chargeBar.maxValue = chargeTime;
            chargeBar.value = chargeTime;
        }


        //UI
        if (!inCooling)
        {
            chargeBar.maxValue = chargeTime;
            chargeBar.value = chargeTime - chargingTime;

            //Change chargeBar's Child Fill's color to green
            chargeColor.color = Color.blue;
        }
        else
        {
            chargeBar.maxValue = chargeCooldown;
            chargeBar.value = coolingTime;

            // Change chargeBar's Child Fill's color to grey
            chargeColor.color = Color.grey;
        }
    }

    //Called from PlayerInput
    public void ChargeButton (InputAction.CallbackContext context)
    {
        if (context.started && coolingTime >= chargeCooldown)
        {
            Debug.Log("Charging");
            //Starts the charge
            charging = true;
            StartCoroutine(Charge());
        }
        else if (context.canceled)
        {
            //Stops the charge
            charging = false;
            Debug.Log("Stopped Charging");
            if (chargingTime >= chargeTime) 
                coolingTime = 0.0f;
            else
                coolingTime = chargeCooldown;

            chargingTime = 0.0f;
        }
        else return;
    }

    public IEnumerator Charge ()
    {
        //Disable playercontrols
        playerController.enabled = false;

        chargingTime = 0.0f;
        Debug.Log($"Charging: {charging}, ChargingTime: {chargingTime}, ChargeTime: {chargeTime}");
        while (charging && chargingTime < chargeTime)
        {
            //Charge for chargeTime
            controller.Move(transform.forward * chargeSpeed * Time.deltaTime);
            chargingTime += Time.deltaTime;
            yield return null;
        }

        //Re-enable playercontrols  
        playerController.enabled = true;

        //Reset charging
        charging = false;
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Collided" + charging);
        if (other.gameObject.CompareTag("Breakable") && charging)
        {
            Debug.Log("Destroyed");
            Destroy(other.gameObject);
        }
    }
}
