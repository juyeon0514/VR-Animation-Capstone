using UnityEngine;

public abstract class AnomalyBase : MonoBehaviour
{
    [Header("이상현상 이름")]
    [SerializeField] private string anomalyName;

    public string AnomalyName => anomalyName;

    public abstract void ActivateAnomaly();
    public abstract void ResetAnomaly();
}
