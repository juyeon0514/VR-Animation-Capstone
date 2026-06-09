using System.Collections;
using TMPro;
using UnityEngine;

public class OutroCreditsUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject outroPanel;
    [SerializeField] private RectTransform creditsText;
    [SerializeField] private TMP_Text creditsTMP;

    [Header("Move")]
    [SerializeField] private float startY = -700f;
    [SerializeField] private float endY = 900f;
    [SerializeField] private float duration = 10f;

    [Header("Credits Text")]
    [TextArea(8, 20)]
    [SerializeField]
    private string creditsContent =
@"제작

팀명 : Team Name

기획 : 이름
프로그래밍 : 이름
아트 : 이름
사운드 : 이름

Thank you for playing.";

    private void Awake()
    {
        if (outroPanel != null)
        {
            outroPanel.SetActive(false);
        }
    }

    public IEnumerator PlayCredits()
    {
        if (outroPanel != null)
        {
            outroPanel.SetActive(true);
        }

        if (creditsTMP != null)
        {
            creditsTMP.text = creditsContent;
        }

        if (creditsText != null)
        {
            creditsText.anchoredPosition = new Vector2(
                creditsText.anchoredPosition.x,
                startY
            );
        }

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            if (creditsText != null)
            {
                float y = Mathf.Lerp(startY, endY, t);
                creditsText.anchoredPosition = new Vector2(
                    creditsText.anchoredPosition.x,
                    y
                );
            }

            yield return null;
        }
    }
}
