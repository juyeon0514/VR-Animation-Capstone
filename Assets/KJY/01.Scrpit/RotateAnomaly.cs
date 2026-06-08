using System.Collections;
using UnityEngine;

public class RotateAnomaly : AnomalyBase
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 targetEulerAngles = new Vector3(0f, 0f, 180f);
    [SerializeField] private float duration = 5f;

    private Quaternion originalRotation;
    private Coroutine rotateRoutine;

    private void Awake()
    {
        if (target != null)
        {
            originalRotation = target.localRotation;
        }
    }

    public override void ActivateAnomaly()
    {
        if (target == null)
        {
            return;
        }

        if (rotateRoutine != null)
        {
            StopCoroutine(rotateRoutine);
        }

        Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);
        rotateRoutine = StartCoroutine(RotateRoutine(originalRotation, targetRotation));
    }

    public override void ResetAnomaly()
    {
        if (rotateRoutine != null)
        {
            StopCoroutine(rotateRoutine);
            rotateRoutine = null;
        }

        if (target != null)
        {
            target.localRotation = originalRotation;
        }
    }

    private IEnumerator RotateRoutine(Quaternion startRot, Quaternion endRot)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            target.localRotation = Quaternion.Slerp(startRot, endRot, t);

            yield return null;
        }

        target.localRotation = endRot;
        rotateRoutine = null;
    }
}
