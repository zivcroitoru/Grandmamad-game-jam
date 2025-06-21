using UnityEngine;

public class GrannyMovementDisabler : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerController playerController;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();
    }

    public void SetEnabled(bool enabled)
    {
        if (characterController != null)
            characterController.enabled = enabled;

        if (playerController != null)
            playerController.enabled = enabled;
    }
}
