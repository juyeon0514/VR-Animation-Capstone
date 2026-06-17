using UnityEngine;
using UnityEngine.UI;

public class VolumeSettingUI : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private const string BGM_VOLUME_KEY = "BGM_VOLUME";
    private const string SFX_VOLUME_KEY = "SFX_VOLUME";

    private void Start()
    {
        float savedBGMVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 1f);
        float savedSFXVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);

        if (bgmSlider != null)
        {
            bgmSlider.value = savedBGMVolume;
            bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = savedSFXVolume;
            sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }

        ApplySavedVolume(savedBGMVolume, savedSFXVolume);
    }

    private void OnBGMVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, value);
        PlayerPrefs.Save();

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetBGMVolume(value);
        }
    }

    private void OnSFXVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, value);
        PlayerPrefs.Save();

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetSFXVolume(value);
        }
    }

    private void ApplySavedVolume(float bgmVolume, float sfxVolume)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetBGMVolume(bgmVolume);
            SoundManager.Instance.SetSFXVolume(sfxVolume);
        }
    }
}
