using UnityEngine;

public class InventoryButtonSound : MonoBehaviour
{
    public void PlayButtonClick()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(SFXType.InventoryButton);
        }
    }
}
