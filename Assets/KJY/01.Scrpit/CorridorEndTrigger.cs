using UnityEngine;

public class CorridorEndTrigger : MonoBehaviour
{
    [SerializeField] private Stage2DoorType corridorType;
    [SerializeField] private Stage2Manager stageManager;

    private bool used;

    private void OnTriggerEnter(Collider other)
    {
        if (used)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        used = true;

        if (stageManager != null)
        {
            stageManager.ReachCorridorEnd(corridorType);
        }
    }

    public void ResetTrigger()
    {
        used = false;
    }
}
