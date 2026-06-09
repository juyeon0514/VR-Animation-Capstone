using System.Collections;
using UnityEngine;

public class MainHallIntroManager : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private DialogueUI dialogueUI;

    [Header("Player Control")]
    [SerializeField] private MonoBehaviour playerMoveScript;
    [SerializeField] private MonoBehaviour playerLookScript;

    [Header("Timing")]
    [SerializeField] private float firstDelay = 1f;
    [SerializeField] private float dialogueInterval = 3f;

    private void Start()
    {
        StartCoroutine(IntroRoutine());
    }

    private IEnumerator IntroRoutine()
    {
        SetPlayerControl(false);

        yield return new WaitForSeconds(firstDelay);

        dialogueUI.ShowDialogue("여친", "미안, 생각보다 늦을 것 같아. 먼저 들어가서 보고 있어.");
        yield return new WaitForSeconds(dialogueInterval);

        dialogueUI.ShowDialogue("나", "괜찮아. 천천히 와. 나 먼저 구경하고 있을게.");
        yield return new WaitForSeconds(dialogueInterval);

        dialogueUI.ShowDialogue("나", "고흐 전시회는 처음인데... 어디부터 봐야 하지?");
        yield return new WaitForSeconds(dialogueInterval);

        dialogueUI.HideDialogue();

        SetPlayerControl(true);
    }

    private void SetPlayerControl(bool canControl)
    {
        if (playerMoveScript != null)
        {
            playerMoveScript.enabled = canControl;
        }

        if (playerLookScript != null)
        {
            playerLookScript.enabled = canControl;
        }
    }
}
