using UnityEngine;

public class ObjectToggleAnomaly : AnomalyBase
{
    [Header("Target")]
    [SerializeField] private GameObject targetObject;

    [Header("Anomaly State")]
    [SerializeField] private bool activeWhenAnomaly = false;

    private bool originalActiveState;

    private void Awake()
    {
        if (targetObject != null)
        {
            originalActiveState = targetObject.activeSelf;
        }
    }

    public override void ActivateAnomaly()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(activeWhenAnomaly);
        }
    }

    public override void ResetAnomaly()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(originalActiveState);
        }
    }
}
