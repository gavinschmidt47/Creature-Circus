using UnityEngine;

public class Melee : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        Debug.Log("Collision");
        if (other.gameObject.tag == "Enemy") {
            Destroy(other.gameObject);
        }
    }
}
