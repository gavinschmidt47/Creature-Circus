using UnityEngine;

public class WinCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (PlayerPrefs.HasKey("levelsCompleted"))
            {
                int levelsCompleted = PlayerPrefs.GetInt("levelsCompleted");
                Debug.Log("Levels Completed: " + levelsCompleted);
                PlayerPrefs.SetInt("levelsCompleted", levelsCompleted);
                PlayerPrefs.Save();
            }
            else
            {
                PlayerPrefs.SetInt("levelsCompleted", 1);
                PlayerPrefs.Save();
            }
        }
    }
}
