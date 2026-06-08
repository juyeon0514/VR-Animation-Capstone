using System.Collections;
using TMPro;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI Instance { get; private set; }

    [SerializeField] private GameObject messagePanel;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private float showTime = 1.5f;

    private Coroutine messageRoutine;

    private void Awake()
    {
        Instance = this;

        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }
    }

    public void ShowMessage(string message)
    {
        if (messagePanel == null || messageText == null)
        {
            Debug.LogWarning("InteractionUI: messagePanel 또는 messageText가 연결되지 않았습니다.");
            return;
        }

        if (messageRoutine != null)
        {
            StopCoroutine(messageRoutine);
        }

        messageRoutine = StartCoroutine(ShowMessageRoutine(message));
    }

    private IEnumerator ShowMessageRoutine(string message)
    {
        messageText.text = message;
        messagePanel.SetActive(true);

        yield return new WaitForSeconds(showTime);

        messagePanel.SetActive(false);
        messageRoutine = null;
    }
}
