using System.Collections;
using UnityEngine;

public class PopOutAnomaly : AnomalyBase
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float duration = 1f;

    private Coroutine popRoutine;

    private void Awake()
    {
        ResetAnomaly();
    }

    public override void ActivateAnomaly()
    {
        if (target == null || startPoint == null || endPoint == null)
        {
            return;
        }

        target.gameObject.SetActive(true);

        if (popRoutine != null)
        {
            StopCoroutine(popRoutine);
        }

        popRoutine = StartCoroutine(PopRoutine());
    }

    public override void ResetAnomaly()
    {
        if (popRoutine != null)
        {
            StopCoroutine(popRoutine);
            popRoutine = null;
        }

        if (target != null && startPoint != null)
        {
            target.position = startPoint.position;
            target.rotation = startPoint.rotation;
            target.gameObject.SetActive(false);
        }
    }

    private IEnumerator PopRoutine()
    {
        float timer = 0f;

        Vector3 startPos = startPoint.position;
        Vector3 endPos = endPoint.position;

        Quaternion startRot = startPoint.rotation;
        Quaternion endRot = endPoint.rotation;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            target.position = Vector3.Lerp(startPos, endPos, t);
            target.rotation = Quaternion.Slerp(startRot, endRot, t);

            yield return null;
        }

        target.position = endPos;
        target.rotation = endRot;
        popRoutine = null;
    }
}
