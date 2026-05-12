using UnityEngine;

public class DrawerOpener : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(0, 0, 0.5f); // 앞으로 얼마나 나올지 (Z축 기준 예시)
    public float openSpeed = 2f; // 열리는 속도

    private Vector3 closedPosition;
    private Vector3 targetPosition;
    private bool isOpening = false;

    void Start()
    {
        // 처음 위치(닫힌 상태) 저장
        closedPosition = transform.localPosition;
        // 열렸을 때의 목표 위치 계산
        targetPosition = closedPosition + openOffset;
    }

    void Update()
    {
        if (isOpening)
        {
            // 현재 위치에서 목표 위치까지 부드럽게 이동
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * openSpeed);

            // 거의 다 도착하면 이동 중지
            if (Vector3.Distance(transform.localPosition, targetPosition) < 0.001f)
            {
                transform.localPosition = targetPosition;
                isOpening = false;
            }
        }
    }

    // 이 함수를 Interactable의 OnAction에서 호출합니다.
    public void OpenDrawer()
    {
        isOpening = true;
        Debug.Log("서랍이 열립니다.");
    }
}
