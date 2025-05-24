using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("Target (player) to follow")]
    public Transform target;

    [Tooltip("Offset from the target position")]
    public Vector3 offset = new Vector3(0, 3, -6);

    [Tooltip("How fast the camera moves")]
    public float positionSmoothSpeed = 5f;

    [Tooltip("How fast the camera rotates")]
    public float rotationSmoothSpeed = 5f;

    private void LateUpdate()
    {
        if (target == null) return;

        // Desired position with offset
        Vector3 desiredPosition = target.position + offset;

        // Smoothly move camera to desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, positionSmoothSpeed * Time.deltaTime);

        // Smoothly rotate camera to match target rotation
        Quaternion desiredRotation = Quaternion.LookRotation(target.forward, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothSpeed * Time.deltaTime);
    }
}
