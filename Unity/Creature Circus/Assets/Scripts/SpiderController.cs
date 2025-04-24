using UnityEngine;

public class SpiderController : MonoBehaviour
{
    public Transform target; // The target to follow
    public GameObject projectilePrefab; 
    public float fireRate = 1f; // Time between shots in seconds
    private float nextFireTime = 0f; // Time when the next shot can be fired
    

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target.position); // Rotate to face the target

        if (Time.time >= nextFireTime)
        {
            Debug.Log("Spider is shooting at the target!"); // Debug message
            Vector3 spawnPosition = transform.position + (transform.forward * 5); // Position in front of the spider
            Instantiate(projectilePrefab, spawnPosition, transform.rotation);
            nextFireTime = Time.time + fireRate;
        }
    }
}
