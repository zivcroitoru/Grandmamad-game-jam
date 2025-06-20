using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 10f;
    public bool isIndoor = true;
    public float rotationSpeed = 10f;

    [Header("References")]
    public Transform cameraTransform; // Drag Main Camera here in Inspector

    private CharacterController controller;
    private Vector3 velocity;
    private float gravity = -9.81f;
    private float speed;
    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // make sure Animator is on Granny
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        speed = isIndoor ? walkSpeed : runSpeed;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // Update animation parameter
        float inputMagnitude = inputDirection.magnitude;
        animator.SetFloat("Speed", inputMagnitude);

        if (inputMagnitude >= 0.1f)
        {
            // Camera-relative direction
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = camForward * vertical + camRight * horizontal;
            moveDirection.Normalize();

            // Rotate Granny
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move
            controller.Move(moveDirection * speed * Time.deltaTime);
        }

        // Apply gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void SetIndoor(bool state)
    {
        isIndoor = state;
    }
}
