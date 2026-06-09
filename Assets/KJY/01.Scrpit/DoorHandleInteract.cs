using UnityEngine;

public class DoorHandleInteract : MonoBehaviour
{
    [SerializeField] private DoorController doorOpen;

    private bool hasOpened = false;

    public void TryOpenDoor()
    {
        if (hasOpened)
        {
            return;
        }

        if (doorOpen == null)
        {
            Debug.LogError("DoorOpen이 연결되지 않았습니다.");
            return;
        }

        hasOpened = true;
        doorOpen.OpenDoor();
    }
}
