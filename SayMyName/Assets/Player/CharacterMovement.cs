using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    [Tooltip("How fast the player accelerates to the target speed (higher = snappier).")]
    public float acceleration = 20f;

    [Header("Mouse Look")]
    public Transform cameraTransform;         // assign your child camera here
    public float mouseSensitivity = 2.0f;
    public float pitchMin = -85f;
    public float pitchMax = 85f;

    [Header("Gravity / Grounding")]
    public float gravity = -30f;              // fairly strong for good grounding
    public float groundedStick = -2f;         // small downward force to keep on ground

    CharacterController controller;
    Vector3 currentVelocity = Vector3.zero;   // for smoothing movement
    float verticalVelocity = 0f;

    float yaw;    // rotation around Y
    float pitch;  // rotation around X (camera)

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        // initialize rotation values
        yaw = transform.eulerAngles.y;
        if (cameraTransform != null) pitch = cameraTransform.localEulerAngles.x;
    }

    void Start()
    {
        // Lock and hide cursor for FPS control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Recommended CharacterController settings:
        // controller.slopeLimit = 45 (defaults usually fine)
        // controller.stepOffset = 0.3f (tweak to taste)
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        if (cameraTransform == null) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);                     // rotate player on Y
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);       // rotate camera on X
    }

    void HandleMovement()
    {
        // Read input (WASD)
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input = input.normalized; // prevent faster diagonal movement

        // Calculate desired move direction in world space, based on player's yaw (camera-forward)
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;
        Vector3 desiredMove = (forward * input.y + right * input.x) * walkSpeed;

        // Smooth acceleration toward desired velocity
        Vector3 horizontalVelocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
        Vector3 targetHorizontal = new Vector3(desiredMove.x, 0f, desiredMove.z);
        horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, targetHorizontal, acceleration * Time.deltaTime);

        // Grounding / Gravity
        if (controller.isGrounded)
        {
            // Avoid small positive vertical velocities that cause 'springing'
            if (verticalVelocity < 0f)
                verticalVelocity = groundedStick; // small negative to keep the controller grounded
        }
        else
        {
            // Apply gravity while airborne
            verticalVelocity += gravity * Time.deltaTime;
        }

        // Combine horizontal and vertical
        currentVelocity = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.z);

        // Move using CharacterController (handles collision and slope behaviour)
        controller.Move(currentVelocity * Time.deltaTime);
    }

    // Optional: call this to unlock cursor (useful for UI)
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
