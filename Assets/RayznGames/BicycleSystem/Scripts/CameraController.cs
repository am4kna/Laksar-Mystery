using UnityEngine;

namespace rayzngames
{
    public class CameraController : MonoBehaviour
    {
        public float mouseSensitivity = 100f;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

            // Apply horizontal rotation (turn the bike/player)
            transform.parent.Rotate(Vector3.up * mouseX);
        }
    }
}
