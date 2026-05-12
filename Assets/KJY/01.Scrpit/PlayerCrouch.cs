using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    [Header("Components")]
    public CharacterController controller; // 플레이어의 Character Controller
    public Transform cameraTransform;      // Main Camera의 Transform

    [Header("Crouch Settings")]
    public float standingHeight = 2.0f;    // 서 있을 때 콜라이더 높이
    public float crouchHeight = 1.0f;      // 앉았을 때 콜라이더 높이
    public float crouchTransitionSpeed = 10f; // 앉고 일어서는 속도 (부드럽게)

    // 카메라의 로컬 Y 위치 (머리 높이)
    private float standingCameraY = 0.8f;
    private float crouchCameraY = 0.2f;

    private bool isCrouching = false;

    void Start()
    {
        // 시작할 때 서 있는 상태의 카메라 높이를 저장해둡니다.
        standingCameraY = cameraTransform.localPosition.y;
        crouchCameraY = standingCameraY - (standingHeight - crouchHeight) / 2f;
    }

    void Update()
    {
        // 왼쪽 Ctrl 키를 누르고 있을 때
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = true;
        }
        // 왼쪽 Ctrl 키에서 손을 뗐을 때
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouching = false;
        }

        // 상태에 따라 부드럽게 높이 변경 (스르륵 앉고 스르륵 일어남)
        if (isCrouching)
        {
            controller.height = Mathf.Lerp(controller.height, crouchHeight, Time.deltaTime * crouchTransitionSpeed);
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, new Vector3(0, crouchCameraY, 0), Time.deltaTime * crouchTransitionSpeed);
        }
        else
        {
            controller.height = Mathf.Lerp(controller.height, standingHeight, Time.deltaTime * crouchTransitionSpeed);
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, new Vector3(0, standingCameraY, 0), Time.deltaTime * crouchTransitionSpeed);
        }
    }
}
