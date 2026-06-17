using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float openAngle = -90f;
    public float openSpeed = 3f;
    private bool isOpening = false;
    private Quaternion targetRotation;

    void Start()
    {
        // 닫힌 상태의 회전값에서 목표 회전값을 미리 계산
        targetRotation = Quaternion.Euler(0, openAngle, 0) * transform.localRotation;
    }

    void Update()
    {
        if (isOpening)
        {
            // 목표 각도까지 부드럽게 회전
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * openSpeed);
        }
    }

    // [중요] Interactable 스크립트의 onAction에서 이 함수를 호출할 겁니다.
    public void OpenDoor()
    {
        isOpening = true;
        SoundManager.Instance.PlaySFX(SFXType.DoorOpen);
        Debug.Log("문이 부드럽게 열리기 시작합니다.");
    }
}
