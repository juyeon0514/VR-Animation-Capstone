using UnityEngine;

public class CorridorEnterTrigger : MonoBehaviour
{
    [SerializeField] private Stage2DoorType corridorType;
    [SerializeField] private Stage2Manager stageManager;
    [SerializeField] private Stage2Door doorToHide;

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

        if (doorToHide != null)
        {
            doorToHide.HideDoor();
        }

        if (stageManager != null)
        {
            stageManager.EnterCorridor(corridorType);
        }
    }

    public void ResetTrigger()
    {
        used = false;
    }
}
