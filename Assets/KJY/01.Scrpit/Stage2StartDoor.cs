using TMPro;
using UnityEngine;

public class Stage2StartDoor : MonoBehaviour
{
    [Header("문")]
    [SerializeField] private Stage2Door stage2Door;

    [Header("진행도 표지판")]
    [SerializeField] private TMP_Text progressText;

    [Header("스테이지 매니저")]
    [SerializeField] private Stage2Manager stage2Manager;

    private bool opened;

    private void Awake()
    {
        if (stage2Door == null)
        {
            stage2Door = GetComponent<Stage2Door>();
        }
    }

    private void OnEnable()
    {
        UpdateProgressSign();
    }

    public void OpenStartDoor()
    {
        if (opened)
        {
            return;
        }

        opened = true;
        UpdateProgressSign();

        if (stage2Door != null)
        {
            stage2Door.OpenDoor();
        }
    }

    public void UpdateProgressSign()
    {
        if (progressText == null || stage2Manager == null)
        {
            return;
        }

        progressText.text = stage2Manager.GetProgressText();
    }

    public void ResetStartDoor()
    {
        opened = false;

        if (stage2Door != null)
        {
            stage2Door.ResetDoor();
        }

        UpdateProgressSign();
    }
}
