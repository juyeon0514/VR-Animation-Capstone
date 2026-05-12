using UnityEngine;

public class MirrorFall : MonoBehaviour
{
    private Rigidbody rb;
    public Vector3 pushForce = new Vector3(0, 0, 5); // 밀어낼 방향과 힘

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // 이 함수를 OnAction에서 호출할 겁니다.
    public void Fall()
    {
        // 1. 물리 고정 해제
        rb.isKinematic = false;

        // 2. 살짝 밀어내기 (거울이 앞으로 고꾸라지게)
        // ForceMode.Impulse는 순간적인 충격을 줍니다.
        rb.AddForce(pushForce, ForceMode.Impulse);

        // 3. 약간의 회전력을 주면 더 실감나게 쓰러집니다.
        rb.AddTorque(new Vector3(5, 0, 0), ForceMode.Impulse);
    }
}
