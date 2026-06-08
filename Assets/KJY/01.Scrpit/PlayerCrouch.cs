using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cameraTransform;

    [Header("Crouch Settings")]
    [SerializeField] private float standingHeight = 2.0f;
    [SerializeField] private float crouchHeight = 1.0f;
    [SerializeField] private float crouchTransitionSpeed = 10f;

    [Header("Camera Settings")]
    [SerializeField] private float crouchCameraOffset = 1.0f;

    [Header("Ceiling Check")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float ceilingCheckRadius = 0.3f;

    private float standingCameraY;
    private float crouchCameraY;

    private Vector3 originalCameraLocalPosition;

    private bool isCrouching;

    private void Start()
    {
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }

        originalCameraLocalPosition = cameraTransform.localPosition;

        standingCameraY = originalCameraLocalPosition.y;
        crouchCameraY = standingCameraY - crouchCameraOffset;

        controller.height = standingHeight;
        controller.center = new Vector3(0f, standingHeight / 2f, 0f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (!IsCeilingBlocked())
            {
                isCrouching = false;
            }
        }

        UpdateCrouch();
    }

    private void UpdateCrouch()
    {
        float targetHeight = isCrouching ? crouchHeight : standingHeight;
        float targetCenterY = targetHeight / 2f;
        float targetCameraY = isCrouching ? crouchCameraY : standingCameraY;

        controller.height = Mathf.Lerp(
            controller.height,
            targetHeight,
            Time.deltaTime * crouchTransitionSpeed
        );

        controller.center = Vector3.Lerp(
            controller.center,
            new Vector3(0f, targetCenterY, 0f),
            Time.deltaTime * crouchTransitionSpeed
        );

        Vector3 targetCameraPosition = new Vector3(
            originalCameraLocalPosition.x,
            targetCameraY,
            originalCameraLocalPosition.z
        );

        cameraTransform.localPosition = Vector3.Lerp(
            cameraTransform.localPosition,
            targetCameraPosition,
            Time.deltaTime * crouchTransitionSpeed
        );
    }

    private bool IsCeilingBlocked()
    {
        Vector3 checkPosition = transform.position + Vector3.up * standingHeight;

        return Physics.CheckSphere(
            checkPosition,
            ceilingCheckRadius,
            obstacleLayer
        );
    }
}
