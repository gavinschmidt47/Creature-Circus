using UnityEngine;

public class Climbable : MonoBehaviour
{

    public float climb = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Commence (float amount)
    {
        climb -= amount;
        if (climb <= 0f)
        {
            Go();
        }
    }

    void Go()
    {
        Destroy(gameObject);
    }
}
