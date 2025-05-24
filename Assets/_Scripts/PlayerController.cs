using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState { Land, Swim }
    public PlayerState currentState = PlayerState.Land;

    [Header("Movement Settings")]
    public float landSpeed = 5f;
    public float swimSpeed = 5f;
    public float swimUpForce = 3f;
    public float diveForce = -2f;
    public float sinkingSpeed = -0.5f;

    [Header("Camera")]
    public Transform cameraPivot; // Assign a parent GameObject of the camera here
    public float cameraYawAngle = 30f;
    public float cameraSmoothSpeed = 5f;

    private PlayerInputActions inputActions;
    private Rigidbody rb;

    private Vector2 moveInput;
    private Vector2 swimInput;

    private Quaternion initialCameraRotation;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody>();

        if (cameraPivot != null)
            initialCameraRotation = cameraPivot.localRotation;
    }

    void OnEnable()
    {
        EnableCurrentActionMap();
    }

    void OnDisable()
    {
        DisableAllActionMaps();
    }

    void Update()
    {
        if (currentState == PlayerState.Land)
        {
            moveInput = inputActions.Land.Move.ReadValue<Vector2>();
        }
        else if (currentState == PlayerState.Swim)
        {
            moveInput = inputActions.Swim.Move.ReadValue<Vector2>();
            swimInput = inputActions.Swim.SwimDirection.ReadValue<Vector2>();
        }

        // UpdateCameraRotation();
    }

    void FixedUpdate()
    {
        if (currentState == PlayerState.Land)
        {
            Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y);
            Vector3 velocity = direction * landSpeed;
            velocity.y = rb.velocity.y; // preserve vertical velocity (gravity)
            rb.velocity = velocity;
        }
        else if (currentState == PlayerState.Swim)
        {
            Vector3 velocity = rb.velocity;
            velocity.x = moveInput.x * swimSpeed;
            velocity.z = moveInput.y * swimSpeed;

            if (swimInput.y > 0)
                velocity.y = swimUpForce;
            else if (swimInput.y < 0)
                velocity.y = diveForce;
            else
                velocity.y = Mathf.MoveTowards(velocity.y, sinkingSpeed, 0.1f);

            rb.velocity = velocity;
        }
    }

    private void UpdateCameraRotation()
    {
        if (cameraPivot == null) return;

        float targetYaw = 0f;

        if (moveInput.x > 0.1f)
            targetYaw = cameraYawAngle;
        else if (moveInput.x < -0.1f)
            targetYaw = -cameraYawAngle;
        else
            targetYaw = 0f;

        Quaternion targetRotation = initialCameraRotation * Quaternion.Euler(0f, targetYaw, 0f);

        cameraPivot.localRotation = Quaternion.Slerp(
            cameraPivot.localRotation,
            targetRotation,
            Time.deltaTime * cameraSmoothSpeed
        );
    }

    public void SwitchState(PlayerState newState)
    {
        currentState = newState;
        EnableCurrentActionMap();
    }

    private void EnableCurrentActionMap()
    {
        DisableAllActionMaps();

        if (currentState == PlayerState.Land)
            inputActions.Land.Enable();
        else if (currentState == PlayerState.Swim)
            inputActions.Swim.Enable();
    }

    private void DisableAllActionMaps()
    {
        inputActions.Land.Disable();
        inputActions.Swim.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            SwitchState(PlayerState.Swim);
        }
        else if (other.name == "Land")
        {
            SwitchState(PlayerState.Land);
        }
    }
}
