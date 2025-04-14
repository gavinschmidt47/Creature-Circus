using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonSceneSwitcher : MonoBehaviour
{
    [Header("Drag the button")]
    public Button targetButton;

    [Header("Enter the scene name you want to switch to")]
    public string sceneToLoad;

    void Start()
    {
        if (targetButton != null)
        {
            targetButton.onClick.AddListener(SwitchScene);
        }
        else
        {
            Debug.LogWarning("assign a button");
        }
    }

    void SwitchScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("assign a scene");
        }
    }
}
