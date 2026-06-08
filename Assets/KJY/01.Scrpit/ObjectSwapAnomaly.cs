using UnityEngine;

public class ObjectSwapAnomaly : AnomalyBase
{
    [SerializeField] private GameObject normalObject;
    [SerializeField] private GameObject anomalyObject;

    private void Awake()
    {
        ResetAnomaly();
    }

    public override void ActivateAnomaly()
    {
        if (normalObject != null)
        {
            normalObject.SetActive(false);
        }

        if (anomalyObject != null)
        {
            anomalyObject.SetActive(true);
        }
    }

    public override void ResetAnomaly()
    {
        if (normalObject != null)
        {
            normalObject.SetActive(true);
        }

        if (anomalyObject != null)
        {
            anomalyObject.SetActive(false);
        }
    }
}
