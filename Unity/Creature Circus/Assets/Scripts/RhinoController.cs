using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RhinoController : MonoBehaviour
{
    //UI
    [Header("UI")]
    [Tooltip("Stamina Slider")]
    public Slider chargeBar;
    [Tooltip("Color of stamina bar when player can charge (insert monocolor image)(size does not matter)")]
    public Image chargeColor;
    [Tooltip("Color of stamina bar when charge is cooling down (insert monocolor image)(size does not matter)")]
    public Image cooldownColor;


    //Settings
    [Header("Charging Settings")]
    [Tooltip("Boost added to speed when charging")]
    public float chargeSpeed = 10.0f;
    [Tooltip("Max time can charge in seconds")]
    public float chargeMax = 2.5f;
    [Tooltip("How fast the charge depletes (1 = 1 second)")]
    public float chargeDeplete = 1f;
    [Tooltip("How much charge comes back each second after use")]
    public float chargeRate = 5.0f;

    private float charge;
    private bool charging = false;
    private bool inCooling = false;
    private Image chargeColorUI;


    //Camera
    [Header("Camera Settings")]
    [Tooltip("Normal follow camera object")]
    public GameObject followCam;
    [Tooltip("Camera object for charging")]
    public GameObject chargeCam;


    //Controller
    private PlayerController playerController;
    private CharacterController controller;

    //Misc.
    public GameObject chargeParticles;
    private Coroutine chargeCoroutine;

    void Start()
    {
        //Gets CharacterController which can use .SimpleMove
        controller = gameObject.GetComponent<CharacterController>();

        //Gets the player controller
        playerController = gameObject.GetComponent<PlayerController>();

        //Sets the UI
        chargeBar.maxValue = chargeMax;
        charge = chargeMax;
        chargeBar.value = charge;
        chargeColorUI = chargeBar.fillRect.GetComponent<Image>();
    }

    void Update()
    {
        //Cooldown
        if (charge < chargeMax && inCooling)
        {
            charge += Time.deltaTime * chargeRate;
            chargeBar.value = charge;
        }
        else if (charge >= chargeMax && inCooling)
        {
            inCooling = false;
            charge = chargeMax;
            chargeBar.value = chargeMax;
            //chargeColorUI.color = chargeColor.color;
        }
        else if (charge <= 0 && !inCooling)
        {
            inCooling = true;
            //chargeColor.color = cooldownColor.color;
        }
        
        if (playerController.paused || playerController.gameOver)
        {
            //If the game is paused, stop charging
            charging = false;
            playerController.invincible = false;
            if (chargeCoroutine != null)
                StopCoroutine(chargeCoroutine);
            followCam.SetActive(true);
            chargeCam.SetActive(false);
        }
    }

    //Called from PlayerInput
    public void ChargeButton (InputAction.CallbackContext context)
    {
        if (context.started && !inCooling)
        {
            //Starts the charge
            charging = true;
            chargeCoroutine = StartCoroutine(Charge());
            followCam.SetActive(false);
            chargeCam.SetActive(true);
            playerController.invincible = true;
        }
        else if (context.canceled)
        {
            //Stops the charge
            charging = false;
            StopCoroutine(chargeCoroutine);
            chargeParticles.SetActive(false);
            followCam.SetActive(true);
            chargeCam.SetActive(false);

            playerController.enabled = true;
            charging = false;
            playerController.invincible = false;
        }
        else return;
    }

    public IEnumerator Charge ()
    {
        //Disable playercontrols
        playerController.enabled = false;
        chargeParticles.SetActive(true);

        while ((charging && charge > 0) || playerController.infStam)
        {
            //Charge for chargeTime
            controller.Move(transform.forward * chargeSpeed * Time.deltaTime);
            if (!playerController.infStam)
            {
                charge -= Time.deltaTime * chargeDeplete;
                chargeBar.value = charge;
            }
            yield return null;
        }

        followCam.SetActive(true);
        chargeCam.SetActive(false);

        chargeParticles.SetActive(false);

        //Re-enable playercontrols  
        playerController.enabled = true;

        //Reset charging
        charging = false;

    }

    private void OnControllerColliderHit(ControllerColliderHit other) {
        if (other.gameObject.CompareTag("Breakable") && charging)
        {
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy") && charging)
        {
            Destroy(other.gameObject);
        }
    }

    public void ChargeBarInfinite(bool on)
    {
        if (on)
        {
            charge = chargeMax;
            chargeBar.value = chargeMax;
            StartCoroutine(PingPongChargeBarColor());
        }
        else
        {
            StopCoroutine(PingPongChargeBarColor());
        }
    }

    private IEnumerator PingPongChargeBarColor()
    {
        while (true)
        {
            chargeColorUI.color = Color.Lerp(Color.white, Color.yellow, Mathf.PingPong(Time.time, 1));
            yield return null;
        }
    }
}
