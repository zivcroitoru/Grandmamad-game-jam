using UnityEngine;

public class MobileInputManager : MonoBehaviour
{
    public FloatingJoystick joystick;
    private bool pickupPressed;

    public void OnPickupPressed() => pickupPressed = true;

    public Vector2 GetJoystickInput() => new Vector2(joystick.Horizontal, joystick.Vertical);
void Update()
{
    Debug.Log("Joystick: " + GetJoystickInput());
}

void Start()
{
    joystick.gameObject.SetActive(true); // show joystick in Editor for testing
}
    public bool ConsumePickup()
    {
        if (pickupPressed)
        {
            pickupPressed = false;
            return true;
        }
        return false;
    }
}
