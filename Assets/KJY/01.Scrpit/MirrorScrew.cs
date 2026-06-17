using System.Collections;
using UnityEngine;

public class MirrorScrew : MonoBehaviour
{
    [SerializeField] private MirrorScrewManager screwManager;

    [Header("나사 빠지는 연출")]
    [SerializeField] private float duration = 1.2f;
    [SerializeField] private float moveDistance = 0.15f;
    [SerializeField] private float rotateSpeed = 720f;

    [Header("빠지는 방향")]
    [SerializeField] private Vector3 localMoveDirection = Vector3.forward;

    private bool isRemoved;

    public void Unscrew()
    {
        if (isRemoved)
        {
            return;
        }

        isRemoved = true;
        StartCoroutine(UnscrewRoutine());
    }

    private IEnumerator UnscrewRoutine()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + transform.TransformDirection(localMoveDirection.normalized) * moveDistance;

        float timer = 0f;

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime, Space.Self);

            yield return null;
        }
        SoundManager.Instance.PlaySFX(SFXType.Nail);

        transform.position = endPos;

        if (screwManager != null)
        {
            screwManager.AddRemovedScrew();
        }

        gameObject.SetActive(false);
    }
}
