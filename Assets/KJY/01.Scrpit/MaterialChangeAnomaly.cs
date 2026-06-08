using UnityEngine;

public class MaterialChangeAnomaly : AnomalyBase
{
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private Material anomalyMaterial;

    private Material originalMaterial;

    private void Awake()
    {
        if (targetRenderer != null)
        {
            originalMaterial = targetRenderer.material;
        }
    }

    public override void ActivateAnomaly()
    {
        if (targetRenderer != null && anomalyMaterial != null)
        {
            targetRenderer.material = anomalyMaterial;
        }
    }

    public override void ResetAnomaly()
    {
        if (targetRenderer != null && originalMaterial != null)
        {
            targetRenderer.material = originalMaterial;
        }
    }
}
