using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float moveSpeed = 10f;
        float rotationSpeed = 500f;

        float horizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float ascend = 0f;

        if (Input.GetKey(KeyCode.Space))
        {
            ascend = moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            ascend = -moveSpeed * Time.deltaTime;
        }

        transform.Translate(horizontal, ascend, vertical);

        float yaw = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float pitch = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        transform.Rotate(pitch, yaw, 0, Space.Self);
        Vector3 currentRotation = transform.localEulerAngles;
        currentRotation.z = 0;
        transform.localEulerAngles = currentRotation;
    }
}
