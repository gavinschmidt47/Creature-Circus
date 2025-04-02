using UnityEngine;

public class HubController : MonoBehaviour
{
    //Gates
    [Header("Gates")]
    public GameObject gate1;
    public GameObject gate2;
    public GameObject gate3;

    //Materials
    [Header("Materials")]
    public Material available;
    public Material unavailable;
    public Material completed;

    private int levelsCompleted;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.HasKey("levelsCompleted"))
        {
            levelsCompleted = PlayerPrefs.GetInt("levelsCompleted");
        }
        else
        {
            PlayerPrefs.SetInt("levelsCompleted", 0);
            PlayerPrefs.Save();

            levelsCompleted = 0;
        }
        Debug.Log("Levels Completed: " + PlayerPrefs.GetInt("levelsCompleted"));

        //Set gates to completed or available based on levels completed
        if (levelsCompleted == 0)
        {
            gate1.GetComponent<Renderer>().material = available;
            gate2.GetComponent<Renderer>().material = unavailable;
            gate3.GetComponent<Renderer>().material = unavailable;
        }
        else if (levelsCompleted == 1)
        {
            gate1.GetComponent<Renderer>().material = completed;
            gate2.GetComponent<Renderer>().material = available;
            gate3.GetComponent<Renderer>().material = unavailable;
        }
        else if (levelsCompleted == 2)
        {
            gate1.GetComponent<Renderer>().material = completed;
            gate2.GetComponent<Renderer>().material = completed;
            gate3.GetComponent<Renderer>().material = available;
        }
        else if (levelsCompleted >= 3)
        {
            gate1.GetComponent<Renderer>().material = completed;
            gate2.GetComponent<Renderer>().material = completed;
            gate3.GetComponent<Renderer>().material = completed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
