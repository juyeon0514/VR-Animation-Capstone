using UnityEngine;

public class Stage2RoomEnterTrigger : MonoBehaviour
{
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
            stageManager.StartRoomAnomaly();
        }
    }

    public void ResetTrigger()
    {
        used = false;
    }
}
