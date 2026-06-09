using System.Collections;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera mainCamera;

    [Header("View Points")]
    [SerializeField] private Transform floorWakeViewPoint;
    [SerializeField] private Transform sitUpViewPoint;
    [SerializeField] private Transform lookBehindHalfViewPoint;
    [SerializeField] private Transform girlfriendLookPoint;
    [SerializeField] private Transform finalGoghLookPoint;
    [SerializeField] private Transform standUpViewPoint;
    [SerializeField] private Transform approachGirlfriendViewPoint;

    [Header("Characters")]
    [SerializeField] private GameObject girlfriend;
    [SerializeField] private GameObject vanGogh;

    [Header("Player Control")]
    [SerializeField] private MonoBehaviour playerMoveScript;
    [SerializeField] private MonoBehaviour playerLookScript;

    [SerializeField] private Transform lookAroundLeftViewPoint;
    [SerializeField] private Transform lookAroundRightViewPoint;

    [Header("Dialogue")]
    [SerializeField] private DialogueUI dialogueUI;

    [Header("Fade")]
    [SerializeField] private FadeUI fadeUI;

    [Header("Outro Credits")]
    [SerializeField] private OutroCreditsUI outroCreditsUI;

    [Header("Return Scene")]
    [SerializeField] private string firstSceneName = "StartScene";

    [Header("Timing")]
    [SerializeField] private float wakeDelay = 1.0f;
    [SerializeField] private float sitUpDuration = 2.0f;
    [SerializeField] private float lookAroundDuration = 1.5f;
    [SerializeField] private float lookGirlfriendDuration = 1.5f;
    [SerializeField] private float finalLookDuration = 1.2f;
    [SerializeField] private float finalSilenceDelay = 2.0f;

    private bool isPlaying;

    private void Start()
    {
        StartEndingCutscene();
    }

    public void StartEndingCutscene()
    {
        if (isPlaying)
        {
            return;
        }

        StartCoroutine(EndingRoutine());
    }

    private IEnumerator EndingRoutine()
    {
        isPlaying = true;

        SetPlayerControl(false);

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (girlfriend != null)
        {
            girlfriend.SetActive(true);
        }

        if (vanGogh != null)
        {
            vanGogh.SetActive(false);
        }

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera가 없습니다.");
            yield break;
        }

        if (floorWakeViewPoint != null)
        {
            mainCamera.transform.position = floorWakeViewPoint.position;
            mainCamera.transform.rotation = floorWakeViewPoint.rotation;
        }

        if (fadeUI != null)
        {
            fadeUI.SetBlack();
            yield return fadeUI.FadeIn();
        }

        yield return new WaitForSeconds(wakeDelay);

        yield return ShowLine("나", "으...", 1.2f);

        yield return MoveCameraTo(lookAroundLeftViewPoint, 1.5f);

        yield return ShowLine("나", "이게 무슨 일이지, 대체...", 2.0f);

        yield return MoveCameraTo(sitUpViewPoint, sitUpDuration);

        yield return MoveCameraTo(lookAroundRightViewPoint, 1.3f);

        yield return ShowLine("나", "맞아... 자기는... 어디 있는 거지?", 2.0f);

        yield return MoveCameraTo(lookBehindHalfViewPoint, 1.0f);

        yield return MoveCameraTo(girlfriendLookPoint, 1.4f);

        yield return ShowLine("나", "자기야...!!", 1.5f);
        
        //yield return MoveCameraTo(standUpViewPoint, 1.5f);

        yield return MoveCameraTo(approachGirlfriendViewPoint, 2.0f);

        yield return ShowLine("나", "보고싶었어 자기야...!!", 2.0f);


        if (girlfriend != null)
        {
            girlfriend.SetActive(false);
        }

        if (vanGogh != null)
        {
            vanGogh.SetActive(true);
        }

        yield return MoveCameraTo(finalGoghLookPoint, finalLookDuration);
        
        yield return new WaitForSeconds(finalSilenceDelay);

        Debug.Log("페이드아웃 시작");

        if (fadeUI != null)
        {
            yield return fadeUI.FadeOut();
            Debug.Log("페이드아웃 완료");
        }
        else
        {
            Debug.LogError("FadeUI가 연결되지 않았습니다.");
        }

        Debug.Log("아웃트로 시작");

        if (outroCreditsUI != null)
        {
            yield return outroCreditsUI.PlayCredits();
            Debug.Log("아웃트로 완료");
        }
        else
        {
            Debug.LogError("OutroCreditsUI가 연결되지 않았습니다.");
        }


        if (StageSceneTransition.Instance != null)
        {
            StageSceneTransition.Instance.LoadSceneFromBlack(firstSceneName);
        }
        else
        {
            Debug.LogError("StageSceneTransition.Instance가 없습니다.");
        }
    }

    private IEnumerator ShowLine(string speaker, string line, float duration)
    {
        if (dialogueUI == null)
        {
            yield break;
        }

        dialogueUI.ShowDialogue(speaker, line);
        yield return new WaitForSeconds(duration);
        dialogueUI.HideDialogue();
    }

    private IEnumerator MoveCameraTo(Transform targetPoint, float duration)
    {
        if (mainCamera == null || targetPoint == null)
        {
            yield break;
        }

        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;

        Vector3 endPosition = targetPoint.position;
        Quaternion endRotation = targetPoint.rotation;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / duration);
            float eased = t * t * (3f - 2f * t);

            mainCamera.transform.position = Vector3.Lerp(startPosition, endPosition, eased);
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, endRotation, eased);

            yield return null;
        }

        mainCamera.transform.position = endPosition;
        mainCamera.transform.rotation = endRotation;
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
