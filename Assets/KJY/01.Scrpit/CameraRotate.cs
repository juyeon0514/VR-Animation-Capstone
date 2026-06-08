using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 100f;

    [Header("References")]
    [SerializeField] private Transform playerBody;

    [Header("Look Limit")]
    [SerializeField] private float minLookAngle = -89f;
    [SerializeField] private float maxLookAngle = 89f;

    private float xRotation = 0f;
    private float zTilt = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minLookAngle, maxLookAngle);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, zTilt);

        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    public void SetCameraTilt(float targetTilt, float speed)
    {
        zTilt = Mathf.Lerp(zTilt, targetTilt, Time.deltaTime * speed);
    }
}
