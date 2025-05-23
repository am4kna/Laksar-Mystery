using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Swimming Settings")]
    public float swimSpeed = 5f;
    public float swimUpForce = 3f;
    public float diveForce = -2f;
    public float sinkingSpeed = -0.5f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 swimInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        var input = new PlayerInputActions();
        input.Swim.Enable();

        input.Swim.Move.performed += OnMove;
        input.Swim.Move.canceled += OnMove;

        input.Swim.SwimDirection.performed += OnSwimDirection;
        input.Swim.SwimDirection.canceled += OnSwimDirection;
    }

    private void OnDisable()
    {
        var input = new PlayerInputActions();
        input.Swim.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnSwimDirection(InputAction.CallbackContext context)
    {
        swimInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;

        // Horizontal
        velocity.x = moveInput.x * swimSpeed;

        // Vertical
        if (swimInput.y > 0)
            velocity.y = swimUpForce;
        else if (swimInput.y < 0)
            velocity.y = diveForce;
        else
            velocity.y = Mathf.MoveTowards(velocity.y, sinkingSpeed, 0.1f);

        rb.velocity = velocity;
    }
}
