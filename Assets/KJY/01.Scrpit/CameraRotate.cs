using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 100f;

    [Header("References")]
    [SerializeField] private Transform playerBody;

    [Header("Normal Look Limit")]
    [SerializeField] private float minLookAngle = -89f;
    [SerializeField] private float maxLookAngle = 89f;

    [Header("Side Lie View")]
    [SerializeField] private float sideLieFixedLookAngle = 60f;
    [SerializeField] private float sideLieRollAngle = 90f;
    [SerializeField] private float sideLieTransitionSpeed = 8f;

    private float xRotation = 0f;
    private float zRotation = 0f;

    private bool isSideLying;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        if (isSideLying)
        {
            xRotation = Mathf.Lerp(
                xRotation,
                sideLieFixedLookAngle,
                Time.deltaTime * sideLieTransitionSpeed
            );

            zRotation = Mathf.Lerp(
                zRotation,
                sideLieRollAngle,
                Time.deltaTime * sideLieTransitionSpeed
            );
        }
        else
        {
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, minLookAngle, maxLookAngle);

            zRotation = Mathf.Lerp(
                zRotation,
                0f,
                Time.deltaTime * sideLieTransitionSpeed
            );
        }

        transform.localRotation = Quaternion.Euler(xRotation, 0f, zRotation);

        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    public void SetSideLieView(bool value)
    {
        isSideLying = value;
    }
}
