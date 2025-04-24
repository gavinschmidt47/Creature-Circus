using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // Projectile speed
    [Tooltip("Speed of the projectile")]
    public float speed = 10f;

    // Projectile lifespan
    [Tooltip("Time before the projectile is destroyed")]
    public float lifespan = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * speed; // Adjust speed as needed
        }

        // Destroy the projectile after a certain time
        Destroy(gameObject, lifespan);
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
