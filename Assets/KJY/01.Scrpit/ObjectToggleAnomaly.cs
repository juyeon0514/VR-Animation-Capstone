using UnityEngine;

public class ObjectToggleAnomaly : AnomalyBase
{
    [SerializeField] private GameObject targetObject;

    private void Awake()
    {
        ResetAnomaly();
    }

    public override void ActivateAnomaly()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
        }
    }

    public override void ResetAnomaly()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }
}
