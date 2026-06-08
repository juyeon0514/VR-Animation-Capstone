using UnityEngine;

public class PlayerPosture : MonoBehaviour
{
    private enum PostureState
    {
        Standing,
        Crouching,
        Prone
    }

    [Header("Components")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CameraRotate cameraRotate;

    [Header("Heights")]
    [SerializeField] private float standingHeight = 2.0f;
    [SerializeField] private float crouchHeight = 1.0f;
    [SerializeField] private float proneHeight = 0.45f;

    [Header("Camera Local Y")]
    [SerializeField] private float standingCameraY = 0.8f;
    [SerializeField] private float crouchCameraY = 0.35f;
    [SerializeField] private float proneCameraY = 0.12f;

    [Header("Camera Tilt")]
    [SerializeField] private float standingCameraZRotation = 0f;
    [SerializeField] private float proneCameraZRotation = 85f;

    [Header("Transition")]
    [SerializeField] private float transitionSpeed = 8f;

    private PostureState currentState = PostureState.Standing;
    private Vector3 originalCameraLocalPosition;

    private void Start()
    {
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }

        if (cameraRotate == null && cameraTransform != null)
        {
            cameraRotate = cameraTransform.GetComponent<CameraRotate>();
        }

        originalCameraLocalPosition = cameraTransform.localPosition;
        standingCameraY = originalCameraLocalPosition.y;
    }

    private void Update()
    {
        HandleInput();
        UpdatePosture();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (currentState != PostureState.Prone)
            {
                currentState = PostureState.Crouching;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (currentState == PostureState.Crouching)
            {
                currentState = PostureState.Standing;
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentState == PostureState.Prone)
            {
                currentState = PostureState.Standing;
            }
            else
            {
                currentState = PostureState.Prone;
            }
        }
    }

    private void UpdatePosture()
    {
        float targetHeight = standingHeight;
        float targetCameraY = standingCameraY;
        float targetTilt = standingCameraZRotation;

        if (currentState == PostureState.Crouching)
        {
            targetHeight = crouchHeight;
            targetCameraY = crouchCameraY;
            targetTilt = standingCameraZRotation;
        }
        else if (currentState == PostureState.Prone)
        {
            targetHeight = proneHeight;
            targetCameraY = proneCameraY;
            targetTilt = proneCameraZRotation;
        }

        controller.height = Mathf.Lerp(
            controller.height,
            targetHeight,
            Time.deltaTime * transitionSpeed
        );

        controller.center = Vector3.Lerp(
            controller.center,
            new Vector3(0f, targetHeight / 2f, 0f),
            Time.deltaTime * transitionSpeed
        );

        Vector3 targetCameraPosition = new Vector3(
            originalCameraLocalPosition.x,
            targetCameraY,
            originalCameraLocalPosition.z
        );

        cameraTransform.localPosition = Vector3.Lerp(
            cameraTransform.localPosition,
            targetCameraPosition,
            Time.deltaTime * transitionSpeed
        );

        if (cameraRotate != null)
        {
            cameraRotate.SetCameraTilt(targetTilt, transitionSpeed);
        }
    }
}
