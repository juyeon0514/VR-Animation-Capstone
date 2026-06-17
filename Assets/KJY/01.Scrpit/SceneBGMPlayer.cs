using UnityEngine;

public class SceneBGMPlayer : MonoBehaviour
{
    [SerializeField] private BGMType bgmType;

    private void Start()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM(bgmType);
        }
    }
}
