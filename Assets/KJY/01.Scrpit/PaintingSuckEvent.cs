using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PaintingSuckEvent : MonoBehaviour
{
    [Header("Target Scene")]
    [SerializeField] private string nextSceneName = "FirstScene";

    [Header("Cinemachine")]
    [SerializeField] private CinemachineCamera playerViewCamera;
    [SerializeField] private CinemachineCamera suckCamera;

    [Header("Suck Targets")]
    [SerializeField] private Transform slowSuckTarget;
    [SerializeField] private Transform finalSuckTarget;

    [Header("Timing")]
    [SerializeField] private float slowPullDuration = 2.0f;
    [SerializeField] private float screamDelay = 0.5f;
    [SerializeField] private float fastSuckDuration = 1.0f;

    [Header("FOV")]
    [SerializeField] private float startFov = 45f;
    [SerializeField] private float midFov = 60f;
    [SerializeField] private float endFov = 95f;

    [Header("Rotation")]
    [SerializeField] private float slowRollAngle = 360f;
    [SerializeField] private float fastRollAngle = 2160f;

    [Header("Black Out")]
    [SerializeField] private CanvasGroup blackFadeGroup;
    [SerializeField, Range(0f, 1f)] private float blackoutStartNormalized = 0.05f;
    [SerializeField, Range(0f, 1f)] private float blackoutEndNormalized = 0.35f;

    [Header("Player Control")]
    [SerializeField] private MonoBehaviour playerMoveScript;
    [SerializeField] private MonoBehaviour playerLookScript;

    [Header("Dialogue")]
    [SerializeField] private DialogueUI dialogueUI;

    private float currentRoll;

    private bool used;

    private void Start()
    {
        if (playerViewCamera != null)
        {
            playerViewCamera.Priority = 10;
        }

        if (suckCamera != null)
        {
            suckCamera.Priority = 0;
        }

        if (blackFadeGroup != null)
        {
            blackFadeGroup.alpha = 0f;
            blackFadeGroup.blocksRaycasts = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (used)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        used = true;
        StartCoroutine(SuckRoutine());
    }

    private IEnumerator SuckRoutine()
    {
        SetPlayerControl(false);

        if (suckCamera == null)
        {
            Debug.LogError("suckCamera가 연결되지 않았습니다.");
            yield break;
        }

        if (slowSuckTarget == null || finalSuckTarget == null)
        {
            Debug.LogError("SlowSuckTarget 또는 FinalSuckTarget이 연결되지 않았습니다.");
            yield break;
        }

        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            suckCamera.transform.position = mainCam.transform.position;
            suckCamera.transform.rotation = mainCam.transform.rotation;
        }

        if (playerViewCamera != null)
        {
            playerViewCamera.Priority = 0;
        }

        suckCamera.Priority = 100;
        SetCameraFov(startFov);
        currentRoll = 0f;

        if (blackFadeGroup != null)
        {
            blackFadeGroup.alpha = 0f;
            blackFadeGroup.blocksRaycasts = true;
        }

        if (dialogueUI != null)
        {
            dialogueUI.ShowDialogue("나", "어... 이 그림, 뭔가 이상한데...");
        }

        yield return MoveCameraPhase(
            slowSuckTarget,
            slowPullDuration,
            startFov,
            midFov,
            slowRollAngle
        );

        yield return new WaitForSeconds(screamDelay);

        if (dialogueUI != null)
        {
            dialogueUI.ShowDialogue("나", "으아아악!!");
        }

        yield return new WaitForSeconds(0.4f);

        yield return MoveCameraPhaseWithBlackout(
            finalSuckTarget,
            fastSuckDuration,
            midFov,
            endFov,
            fastRollAngle
        );

        if (dialogueUI != null)
        {
            dialogueUI.HideDialogue();
        }

        if (StageSceneTransition.Instance != null)
        {
            StageSceneTransition.Instance.LoadSceneFromBlack(nextSceneName);
        }
        else
        {
            Debug.LogError("StageSceneTransition.Instance가 없습니다.");
        }
    }

    private IEnumerator MoveCameraPhase(
        Transform target,
        float duration,
        float fromFov,
        float toFov,
        float targetRoll)
    {
        Vector3 startPosition = suckCamera.transform.position;
        Quaternion startRotation = suckCamera.transform.rotation;

        Vector3 endPosition = target.position;
        Quaternion endRotation = target.rotation;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            float eased = t * t * (3f - 2f * t);

            suckCamera.transform.position = Vector3.Lerp(startPosition, endPosition, eased);

            Quaternion baseRot = Quaternion.Slerp(startRotation, endRotation, eased);
            float rollAmount = Mathf.Lerp(0f, targetRoll, t);
            Quaternion rollRot = Quaternion.AngleAxis(rollAmount, Vector3.forward);

            suckCamera.transform.rotation = baseRot * rollRot;

            float currentFov = Mathf.Lerp(fromFov, toFov, eased);
            SetCameraFov(currentFov);

            yield return null;
        }

        suckCamera.transform.position = endPosition;
        suckCamera.transform.rotation = endRotation * Quaternion.AngleAxis(targetRoll, Vector3.forward);
        SetCameraFov(toFov);
    }

    private IEnumerator MoveCameraPhaseWithBlackout(
     Transform target,
     float duration,
     float fromFov,
     float toFov,
     float rollAddAmount)
    {
        Vector3 startPosition = suckCamera.transform.position;
        Quaternion startRotation = suckCamera.transform.rotation;

        Vector3 endPosition = target.position;
        Quaternion endRotation = target.rotation;

        float startRoll = currentRoll;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / duration);

            // 이동은 부드럽게
            float moveT = t * t * (3f - 2f * t);

            suckCamera.transform.position = Vector3.Lerp(startPosition, endPosition, moveT);

            Quaternion baseRot = Quaternion.Slerp(startRotation, endRotation, moveT);

            // 회전은 끊기지 않게 일정 속도
            float rollAmount = startRoll + rollAddAmount * t;
            Quaternion rollRot = Quaternion.AngleAxis(rollAmount, Vector3.forward);

            suckCamera.transform.rotation = baseRot * rollRot;

            float currentFov = Mathf.Lerp(fromFov, toFov, moveT);
            SetCameraFov(currentFov);

            if (blackFadeGroup != null)
            {
                float fadeT = 0f;

                if (t >= blackoutStartNormalized)
                {
                    fadeT = Mathf.InverseLerp(blackoutStartNormalized, 1f, t);
                }

                blackFadeGroup.alpha = fadeT;
            }

            yield return null;
        }

        currentRoll = startRoll + rollAddAmount;

        suckCamera.transform.position = endPosition;
        suckCamera.transform.rotation = endRotation * Quaternion.AngleAxis(currentRoll, Vector3.forward);
        SetCameraFov(toFov);

        if (blackFadeGroup != null)
        {
            blackFadeGroup.alpha = 1f;
        }
    }

    private void SetCameraFov(float fov)
    {
        LensSettings lens = suckCamera.Lens;
        lens.FieldOfView = fov;
        suckCamera.Lens = lens;
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
