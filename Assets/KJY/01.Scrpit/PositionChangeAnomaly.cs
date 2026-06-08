using UnityEngine;

public class PositionChangeAnomaly : AnomalyBase
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform anomalyPosition;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Awake()
    {
        if (target != null)
        {
            originalPosition = target.position;
            originalRotation = target.rotation;
        }
    }

    public override void ActivateAnomaly()
    {
        if (target == null || anomalyPosition == null)
        {
            return;
        }

        target.position = anomalyPosition.position;
        target.rotation = anomalyPosition.rotation;
    }

    public override void ResetAnomaly()
    {
        if (target == null)
        {
            return;
        }

        target.position = originalPosition;
        target.rotation = originalRotation;
    }
}
