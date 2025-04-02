using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    //UI Elements
    [Header("UI Elements")]
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject aboutPanel;

    public void Start()
    {
        // Initialize the main menu
        mainMenuPanel.SetActive(true);
        //optionsPanel.SetActive(false);
        aboutPanel.SetActive(false);
    }

    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainHub");
    }

    public void Options()
    {
        // Load the options scene or show options menu
        Debug.Log("Options menu is not implemented yet.");
    }

    public void About()
    {
        //Load the about menu
        mainMenuPanel.SetActive(false);
        //optionsPanel.SetActive(false);
        aboutPanel.SetActive(true);
    }


    public void Quit()
    {
        Application.Quit();
        Debug.Log("Game is exiting...");
    }

    public void BackToMainMenu()
    {
        // Return to the main menu
        mainMenuPanel.SetActive(true);
        //optionsPanel.SetActive(false);
        aboutPanel.SetActive(false);
    }
}
