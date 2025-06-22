using UnityEngine;

public class ForceJoystickActivator : MonoBehaviour
{
    public GameObject moveJoystick;
    public GameObject cameraJoystick;

    void Awake()
    {
        if (!Application.isMobilePlatform)
        {
            Debug.Log("[JoystickActivator] Not mobile — skipping joystick activation.");
            return;
        }

        Debug.Log("[JoystickActivator] Awake() called on mobile");

        ForceActivate(moveJoystick, "Move Joystick");
        ForceActivate(cameraJoystick, "Camera Joystick");
    }

    void Start()
    {
        if (!Application.isMobilePlatform) return;

        Debug.Log("[JoystickActivator] Start() called on mobile");

        ForceActivate(moveJoystick, "Move Joystick");
        ForceActivate(cameraJoystick, "Camera Joystick");
    }

    void Update()
    {
        if (!Application.isMobilePlatform) return;

        if (moveJoystick == null)
        {
            Debug.LogWarning("[JoystickActivator] Move joystick reference is NULL in Update!");
        }
        else if (!moveJoystick.activeInHierarchy)
        {
            Debug.LogWarning("[JoystickActivator] Move joystick is NOT active in hierarchy. Forcing activate...");
            ForceActivate(moveJoystick, "Move Joystick");
        }

        if (cameraJoystick == null)
        {
            Debug.LogWarning("[JoystickActivator] Camera joystick reference is NULL in Update!");
        }
        else if (!cameraJoystick.activeInHierarchy)
        {
            Debug.LogWarning("[JoystickActivator] Camera joystick is NOT active in hierarchy. Forcing activate...");
            ForceActivate(cameraJoystick, "Camera Joystick");
        }
    }

    void ForceActivate(GameObject root, string label)
    {
        if (root == null)
        {
            Debug.LogError($"[JoystickActivator] {label} reference is NULL! Skipping activation.");
            return;
        }

        Debug.Log($"[JoystickActivator] Forcing activation for {label} root: {root.name}");

        if (!root.activeSelf)
        {
            root.SetActive(true);
            Debug.Log($"[JoystickActivator] {label}: Root GameObject activated.");
        }

        int childCount = 0;
        foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
        {
            if (!child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(true);
                Debug.Log($"[JoystickActivator] {label}: Activated child: {child.name}");
            }
            childCount++;
        }
        Debug.Log($"[JoystickActivator] {label}: Total children processed: {childCount}");

        int canvasGroupCount = 0;
        foreach (CanvasGroup cg in root.GetComponentsInChildren<CanvasGroup>(true))
        {
            cg.alpha = 1f;
            cg.interactable = true;
            cg.blocksRaycasts = true;
            Debug.Log($"[JoystickActivator] {label}: CanvasGroup updated on {cg.gameObject.name}");
            canvasGroupCount++;
        }
        Debug.Log($"[JoystickActivator] {label}: Total CanvasGroups fixed: {canvasGroupCount}");
    }
}
