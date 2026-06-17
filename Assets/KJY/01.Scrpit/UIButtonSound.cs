using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    public void PlayButtonClick()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(SFXType.ButtonClick);
        }
    }
}
