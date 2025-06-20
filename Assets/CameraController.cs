using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;      // Your player
    public Transform pivot;       // The vertical pivot (CameraPivot)
    public float distance = 5f;   // Distance behind the player
    public float mouseSensitivity = 2f;
    public float verticalClampMin = -30f;
    public float verticalClampMax = 60f;

    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yaw = transform.eulerAngles.y;
        pitch = pivot.localEulerAngles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Follow the player
        transform.position = target.position;

        // Mouse input
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, verticalClampMin, verticalClampMax);

        // Apply rotations
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);          // Yaw
        pivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);       // Pitch

        // Reposition the camera
        Camera.main.transform.localPosition = new Vector3(0f, 0f, -distance);
        Camera.main.transform.localRotation = Quaternion.identity;
    }
}
