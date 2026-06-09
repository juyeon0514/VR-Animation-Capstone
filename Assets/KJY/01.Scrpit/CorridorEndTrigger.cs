using UnityEngine;

public class CorridorEndTrigger : MonoBehaviour
{
    [SerializeField] private Stage2DoorType corridorType;
    [SerializeField] private Stage2Manager stageManager;

    private bool used;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("통로 입구 트리거 감지: " + other.name);


        if (used)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            Debug.LogWarning("Player 태그가 아니라 무시됨: " + other.tag);
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
