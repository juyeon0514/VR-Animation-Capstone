using System.Collections;
using UnityEngine;

public class ScaleAnomaly : AnomalyBase
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f);
    [SerializeField] private float duration = 8f;

    private Vector3 originalScale;
    private Coroutine scaleRoutine;

    private void Awake()
    {
        if (target != null)
        {
            originalScale = target.localScale;
        }
    }

    public override void ActivateAnomaly()
    {
        if (target == null)
        {
            return;
        }

        if (scaleRoutine != null)
        {
            StopCoroutine(scaleRoutine);
        }

        scaleRoutine = StartCoroutine(ScaleRoutine(originalScale, targetScale));
    }

    public override void ResetAnomaly()
    {
        if (scaleRoutine != null)
        {
            StopCoroutine(scaleRoutine);
            scaleRoutine = null;
        }

        if (target != null)
        {
            target.localScale = originalScale;
        }
    }

    private IEnumerator ScaleRoutine(Vector3 startScale, Vector3 endScale)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            target.localScale = Vector3.Lerp(startScale, endScale, t);

            yield return null;
        }

        target.localScale = endScale;
        scaleRoutine = null;
    }
}
