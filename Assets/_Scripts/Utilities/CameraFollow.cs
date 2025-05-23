using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target to follow")]
    public Transform target;

    [Header("Offset from target")]
    public Vector3 offset = new Vector3(0, 0, -10f);

    [Header("Smooth Follow")]
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        // World position of the target + offset
        Vector3 desiredPosition = target.position + offset;

        // Smooth camera movement
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
