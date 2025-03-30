using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //play button function
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //quit button function
    public void QuitGame()
    {
        Debug.Log("game has been quit");
        Application.Quit();
    }
}
