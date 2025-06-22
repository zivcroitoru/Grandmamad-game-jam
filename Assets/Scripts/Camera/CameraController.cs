using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    public Transform target;      // Player
    public Transform pivot;       // Pivot for pitch rotation
    public float distance = 5f;   // Camera distance behind player

    [Header("Sensitivity")]
    public float mouseSensitivity = 2f;
    public float verticalClampMin = -30f;
    public float verticalClampMax = 60f;

    [Header("Mobile Support")]
    public FloatingJoystick lookJoystick;   // Right joystick for camera
    public bool autoDetectMobile = true;

    private float yaw = 0f;
    private float pitch = 0f;
    private bool isMobile;

    void Start()
    {
        isMobile = Application.isMobilePlatform;

        if (!isMobile)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        yaw = transform.eulerAngles.y;
        pitch = pivot.localEulerAngles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Follow player
        transform.position = target.position;

        float inputX = 0f, inputY = 0f;

        if (isMobile && lookJoystick != null)
        {
            Vector2 lookInput = lookJoystick.Direction; // already normalized
            inputX = lookInput.x * mouseSensitivity * 5f; // multiply for mobile sensitivity
            inputY = lookInput.y * mouseSensitivity * 5f;
        }
        else
        {
            inputX = Input.GetAxis("Mouse X") * mouseSensitivity;
            inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        }

        yaw += inputX;
        pitch -= inputY;
        pitch = Mathf.Clamp(pitch, verticalClampMin, verticalClampMax);

        // Apply rotations
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        pivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        // Reposition camera
        Camera.main.transform.localPosition = new Vector3(0f, 0f, -distance);
        Camera.main.transform.localRotation = Quaternion.identity;
    }
}
