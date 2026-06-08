using UnityEngine;

public class MirrorFall : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private Vector3 pushForce = new Vector3(0, 0, 5);
    [SerializeField] private Vector3 torqueForce = new Vector3(5, 0, 0);

    private bool hasFallen;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    public void Fall()
    {
        if (hasFallen)
        {
            return;
        }

        hasFallen = true;

        if (rb == null)
        {
            Debug.LogError("MirrorFall: Rigidbody가 없습니다.");
            return;
        }

        Debug.Log("거울 물리 활성화됨");
        rb.isKinematic = false;
        rb.AddForce(pushForce, ForceMode.Impulse);
        rb.AddTorque(torqueForce, ForceMode.Impulse);
    }

}
