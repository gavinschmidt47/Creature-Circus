using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    //PlayerController
    [Header("PlayerController")]
    [Tooltip("Drag Player GameObject here")]
    public PlayerController player;

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
    public TextMeshProUGUI timeText;

    private float timeLeft = 60f;

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
        sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Rhino")
            StartCoroutine(Timer());
        
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
                infT.isOn = player.infStam;

                //Disable Player
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

        if (sceneName == "Rhino")
        {
            //Time Left
            timeLeft -= Time.deltaTime;
            timeText.text = "Time Left: " + Mathf.Round(timeLeft).ToString();
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

    private IEnumerator Timer()
    {
        //Game Ends after 1 minute
        yield return new WaitForSeconds(60f);
        if (!player.gameOver)
        {
            player.gameOver = true;
            player.paused = true;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            losePanel.SetActive(true);
        }
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
        if (sceneName == "Rhino" && PlayerPrefs.GetInt("RhinoWin", 0) != 1)
        {
            PlayerPrefs.SetInt("RhinoWin", 1);
            PlayerPrefs.Save();
        }
        else if (sceneName == "Monkey" && PlayerPrefs.GetInt("MonkeyWin", 0) != 1)
        {
            PlayerPrefs.SetInt("MonkeyWin", 1);
            PlayerPrefs.Save();
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

    public void ReRhino()
    {
        SceneManager.LoadScene("Rhino");
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ReMonkey()
    {
        SceneManager.LoadScene("Monkey");
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}