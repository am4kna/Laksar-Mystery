using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 1.5f;
    public Transform playerCamera;

    private float cameraPitch = 0f;
    private Rigidbody rb;
    private Vector3 movementInput;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // prevent capsule from tipping over
    }

    void Update()
    {
        // Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * mouseX);

        // Movement input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        movementInput = (transform.right * x + transform.forward * z).normalized;
    }

    void FixedUpdate()
    {
        // Apply movement using Rigidbody
        Vector3 velocity = movementInput * speed;
        Vector3 rbVelocity = new Vector3(velocity.x, rb.velocity.y, velocity.z); // keep Y velocity (gravity)
        rb.velocity = rbVelocity;
    }
}
