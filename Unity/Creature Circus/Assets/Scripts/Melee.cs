using UnityEngine;

public class Melee : MonoBehaviour
{
    private AudioSource hitSound;
    private void Start() {
        // Get the Audio component attached to the same GameObject
        hitSound = GetComponent<AudioSource>();
        if (hitSound == null) {
            Debug.LogError("AudioSource component not found on this GameObject.");
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Enemy") {
            // Play the hit sound
            hitSound.PlayOneShot(hitSound.clip);
            // Destroy the enemy object
            Destroy(other.gameObject);
        }
    }
}
