using UnityEngine;

public class DoorHandleInstaller : MonoBehaviour
{
    [Header("Required Item")]
    [SerializeField] private Item requiredHandleItem;

    [Header("Handle Object")]
    [SerializeField] private GameObject handleVisual;

    [Header("Inventory")]
    [SerializeField] private Inventory inventory;

    private bool isHandleInstalled = false;

    public void TryInstallHandle()
    {
        if (isHandleInstalled)
        {
            if (InteractionUI.Instance != null)
            {
                InteractionUI.Instance.ShowMessage("이미 손잡이가 붙어 있습니다.");
            }

            return;
        }

        if (inventory == null)
        {
            Debug.LogError("Inventory가 연결되지 않았습니다.");
            return;
        }

        if (requiredHandleItem == null)
        {
            Debug.LogError("Required Handle Item이 연결되지 않았습니다.");
            return;
        }

        if (!inventory.HasItem(requiredHandleItem))
        {
            if (InteractionUI.Instance != null)
            {
                InteractionUI.Instance.ShowMessage("문 손잡이가 필요합니다.");
            }

            return;
        }

        InstallHandle();
    }

    private void InstallHandle()
    {
        isHandleInstalled = true;

        if (handleVisual != null)
        {
            handleVisual.SetActive(true);
        }

        if (InteractionUI.Instance != null)
        {
            InteractionUI.Instance.ShowMessage("문 손잡이를 붙였습니다.");
        }
    }

    public bool IsHandleInstalled()
    {
        return isHandleInstalled;
    }
}
