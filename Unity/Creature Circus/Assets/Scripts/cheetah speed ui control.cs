using UnityEngine;
using TMPro;

public class PlayerSpeedDisplay : MonoBehaviour
{
    
    public Rigidbody playerRb;

    
    public TMP_Text speedText;

    void Update()
    {
        if (playerRb != null && speedText != null)
        {
            float speed = playerRb.velocity.magnitude; // Get current speed
            speedText.text = "Speed: " + speed.ToString("F2") + " m/s";
        }
    }
}
