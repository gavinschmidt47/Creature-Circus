using UnityEngine;
using System.Collections;

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

    //Particles
    [Header("Particles")]
    public GameObject gate1Particles;
    public GameObject gate2Particles;
    public GameObject gate3Particles;

    //Animation time
    [Header("Animation Time")]
    public float firstWaitTime = 2f;
    public float secondWaitTime = 2f;   
    public float thirdWaitTime = 2f;

    private int levelsCompleted;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gate1.GetComponent<Renderer>().material = unavailable;
        gate2.GetComponent<Renderer>().material = unavailable;
        gate3.GetComponent<Renderer>().material = unavailable;

        if (PlayerPrefs.GetInt("RhinoWin") == 1)
        {
            gate1.GetComponent<Renderer>().material = available;
        }
        if (PlayerPrefs.GetInt("MonkeyWin") == 1)
        {
            gate2.GetComponent<Renderer>().material = available;
        }
        if (PlayerPrefs.GetInt("CheetahWin") == 1)
        {
            gate3.GetComponent<Renderer>().material = available;
        }

        if (PlayerPrefs.GetInt("RhinoWin") == 1 && PlayerPrefs.GetInt("MonkeyWin") == 1 && PlayerPrefs.GetInt("CheetahWin") == 1)
        {
            StartCoroutine(CompleteHub());
        }
    }

    private IEnumerator CompleteHub()
    {
        yield return new WaitForSeconds(firstWaitTime);
        gate2.GetComponent<Renderer>().material = completed;
        gate2Particles.SetActive(true);
        yield return new WaitForSeconds(secondWaitTime);
        gate3.GetComponent<Renderer>().material = completed;
        gate3Particles.SetActive(true);
        yield return new WaitForSeconds(thirdWaitTime);
        gate1.GetComponent<Renderer>().material = completed;
        gate1Particles.SetActive(true);
    }
}
