using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CheetahGameController : MonoBehaviour
{
    //PlayerController
    [Header("PlayerController")]
    [Tooltip("Drag Player GameObject here")]
    public CheetahController player;

    //Input in-gamne
    [Header("Input")]
    [Tooltip("Input Action Asset")]
    public InputActionAsset playerControls;

    private InputAction pause;

    //UI
    [Header("UI")]
    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public Toggle invT;
    public Toggle infT;
        public Toggle autT;

    //Misc.
    private string sceneName;

    
    void Awake ()
    {
        pause = playerControls.FindAction("Cancel");
    }

    void OnEnable ()
    {
        pause.Enable();
    }
    void OnDisable ()
    {
        pause.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        //Disable Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        player.gameOver = false;
        player.paused = false;
        player.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Pause
        if (pause.triggered)
        {
            if (!player.paused && !player.gameOver )
            {
                //Freeze Time
                Time.timeScale = 0;

                //Enable Cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                //Enable UI
                pausePanel.SetActive(true);
                player.paused = true;

                //Set toggles
                invT.isOn = player.invincible;
                autT.isOn = player.autoHit;

                player.enabled = false;
            }
            else if (player.paused && !player.gameOver)
            {
                Resume();
            }
            else
            {
                ToHub();
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;

        //Disable Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pausePanel.SetActive(false);

        player.paused = false;
        player.enabled = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToHub()
    {
        SceneManager.LoadScene("MainHub");
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player.gameObject.SetActive(true);
        player.paused = false;
        player.gameOver = false;
        player.enabled = true;
    }

    public void WinGame()
    {
        player.gameOver = true;
        player.paused = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        winPanel.SetActive(true);
        player.gameObject.SetActive(false);

        // Save progress using PlayerPrefs
        if (PlayerPrefs.GetInt("CheetahWin") != 1)
        {
            PlayerPrefs.SetInt("CheetahWin", 1); // Mark the game as won (1 for true)
            PlayerPrefs.Save(); // Save the changes
        }
    }

    public void LoseGame()
    {
        player.gameOver = true;
        player.paused = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        losePanel.SetActive(true);
        player.gameObject.SetActive(false);
    }

    public void ReCheetah()
    {
        SceneManager.LoadScene("Cheetah");
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}