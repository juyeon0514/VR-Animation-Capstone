using UnityEngine;
using UnityEngine.Audio;

public enum BGMType
{
    ArtMuseum,
    Stage1,
    Stage2,
    Ending
}

public enum SFXType
{
    PlayerWalk,
    DoorOpen,
    DoorClose,
    ItemGet,
    ItemEquip,
    InventoryButton,
    ButtonClick
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("BGM Clips")]
    [SerializeField] private AudioClip artMuseumBGM;
    [SerializeField] private AudioClip stage1BGM;
    [SerializeField] private AudioClip stage2BGM;
    [SerializeField] private AudioClip endingBGM;

    [Header("SFX Clips")]
    [SerializeField] private AudioClip[] playerWalkSFXList;
    [SerializeField] private AudioClip doorOpenSFX;
    [SerializeField] private AudioClip doorCloseSFX;
    [SerializeField] private AudioClip itemGetSFX;
    [SerializeField] private AudioClip itemEquipSFX;
    [SerializeField] private AudioClip inventoryButtonSFX;
    [SerializeField] private AudioClip buttonClickSFX;
    
    private BGMType? currentBGMType;
    
    private int currentWalkIndex;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (bgmSource != null)
        {
            bgmSource.loop = true;
        }

        float savedBGMVolume = PlayerPrefs.GetFloat("BGM_VOLUME", 1f);
        float savedSFXVolume = PlayerPrefs.GetFloat("SFX_VOLUME", 1f);

        SetBGMVolume(savedBGMVolume);
        SetSFXVolume(savedSFXVolume);
    }

    public void PlayBGM(BGMType bgmType)
    {
        if (bgmSource == null)
        {
            Debug.LogError("BGM AudioSource가 연결되지 않았습니다.");
            return;
        }

        if (currentBGMType == bgmType)
        {
            return;
        }

        AudioClip clip = GetBGMClip(bgmType);

        if (clip == null)
        {
            Debug.LogWarning("BGM Clip이 비어 있습니다: " + bgmType);
            return;
        }

        currentBGMType = bgmType;
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource == null)
        {
            return;
        }

        bgmSource.Stop();
        currentBGMType = null;
    }

    public void PlaySFX(SFXType sfxType)
    {
        if (sfxSource == null)
        {
            Debug.LogError("SFX AudioSource가 연결되지 않았습니다.");
            return;
        }

        AudioClip clip = GetSFXClip(sfxType);

        if (clip == null)
        {
            Debug.LogWarning("SFX Clip이 비어 있습니다: " + sfxType);
            return;
        }

        sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX(SFXType sfxType, float volume)
    {
        if (sfxSource == null)
        {
            return;
        }

        AudioClip clip = GetSFXClip(sfxType);

        if (clip == null)
        {
            return;
        }

        sfxSource.PlayOneShot(clip, volume);
    }

    public void SetBGMVolume(float volume)
    {
        SetMixerVolume("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        SetMixerVolume("SFXVolume", volume);
    }

    private void SetMixerVolume(string parameterName, float volume)
    {
        if (audioMixer == null)
        {
            Debug.LogError("AudioMixer가 연결되지 않았습니다.");
            return;
        }

        volume = Mathf.Clamp01(volume);

        float dB;

        if (volume <= 0.0001f)
        {
            dB = -80f;
        }
        else
        {
            dB = Mathf.Log10(volume) * 20f;
        }

        audioMixer.SetFloat(parameterName, dB);
    }

    private AudioClip GetBGMClip(BGMType bgmType)
    {
        switch (bgmType)
        {
            case BGMType.ArtMuseum:
                return artMuseumBGM;

            case BGMType.Stage1:
                return stage1BGM;

            case BGMType.Stage2:
                return stage2BGM;

            case BGMType.Ending:
                return endingBGM;

            default:
                return null;
        }
    }

    private AudioClip GetSFXClip(SFXType sfxType)
    {
        switch (sfxType)
        {
            case SFXType.PlayerWalk:
                return GetNextWalkClip();

            case SFXType.DoorOpen:
                return doorOpenSFX;

            case SFXType.DoorClose:
                return doorCloseSFX;

            case SFXType.ItemGet:
                return itemGetSFX;

            case SFXType.ItemEquip:
                return itemEquipSFX;

            case SFXType.InventoryButton:
                return inventoryButtonSFX;

            case SFXType.ButtonClick:
                return buttonClickSFX;

            default:
                return null;
        }
    }

    private AudioClip GetNextWalkClip()
    {
        if (playerWalkSFXList == null || playerWalkSFXList.Length == 0)
        {
            return null;
        }

        AudioClip clip = playerWalkSFXList[currentWalkIndex];

        currentWalkIndex++;

        if (currentWalkIndex >= playerWalkSFXList.Length)
        {
            currentWalkIndex = 0;
        }

        return clip;
    }
}
