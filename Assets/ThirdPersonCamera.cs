using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;       // The player
    public Vector3 offset = new Vector3(0f, 3f, -5f);
    public float rotationSpeed = 3f;
    public float smoothSpeed = 0.1f;

    private float yaw;
    private float pitch = 10f;

    void LateUpdate()
    {
        if (!target) return;

        // Mouse input
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -20f, 60f);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position + rotation * offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.LookAt(target.position + Vector3.up * 1.5f); // look at player's head
    }
}
