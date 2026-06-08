using UnityEngine;

public class PlayerPosture : MonoBehaviour
{
    private enum PostureState
    {
        Standing,
        Crouching,
        SideLying
    }

    [Header("Components")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CameraRotate cameraRotate;

    [Header("Character Controller Height")]
    [SerializeField] private float standingHeight = 2.0f;
    [SerializeField] private float crouchHeight = 1.0f;
    [SerializeField] private float sideLieHeight = 0.4f;

    [Header("Camera Local Position")]
    [SerializeField] private float crouchCameraY = 0.35f;
    [SerializeField] private float sideLieCameraY = -1.0f;

    [Header("Transition")]
    [SerializeField] private float transitionSpeed = 8f;

    private PostureState currentState = PostureState.Standing;

    private Vector3 originalCameraLocalPosition;
    private float standingCameraY;

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

        if (controller != null)
        {
            controller.height = standingHeight;
            controller.center = new Vector3(0f, standingHeight / 2f, 0f);
        }
    }

    private void Update()
    {
        HandleInput();
        UpdatePosture();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentState == PostureState.SideLying)
            {
                currentState = PostureState.Standing;

                if (cameraRotate != null)
                {
                    cameraRotate.SetSideLieView(false);
                }
            }
            else
            {
                currentState = PostureState.SideLying;

                if (cameraRotate != null)
                {
                    cameraRotate.SetSideLieView(true);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (currentState != PostureState.SideLying)
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
    }

    private void UpdatePosture()
    {
        float targetHeight = standingHeight;
        float targetCameraY = standingCameraY;

        if (currentState == PostureState.Crouching)
        {
            targetHeight = crouchHeight;
            targetCameraY = crouchCameraY;
        }
        else if (currentState == PostureState.SideLying)
        {
            targetHeight = sideLieHeight;
            targetCameraY = sideLieCameraY;
        }

        if (controller != null)
        {
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
        }

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
    }
}
