using UnityEngine;

public class FlipAnomaly : AnomalyBase
{
    [SerializeField] private Transform target;

    private Vector3 originalScale;

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

        Vector3 flippedScale = originalScale;
        flippedScale.x *= -1f;

        target.localScale = flippedScale;
    }

    public override void ResetAnomaly()
    {
        if (target != null)
        {
            target.localScale = originalScale;
        }
    }
}
