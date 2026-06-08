using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Stage2Manager : MonoBehaviour
{
    [Header("진행 설정")]
    [SerializeField] private int clearRequiredCount = 5;

    [Tooltip("이상현상이 없는 방이 나올 확률입니다. 0.35면 35% 확률로 정상 방입니다.")]
    [Range(0f, 1f)]
    [SerializeField] private float noAnomalyChance = 0.35f;

    [Header("이상현상 목록")]
    [SerializeField] private AnomalyBase[] anomalies;

    [Header("문")]
    [SerializeField] private Stage2Door leftDoor;
    [SerializeField] private Stage2Door rightDoor;

    [Header("통로 입구 트리거")]
    [SerializeField] private CorridorEnterTrigger leftCorridorEnterTrigger;
    [SerializeField] private CorridorEnterTrigger rightCorridorEnterTrigger;

    [Header("통로 끝 트리거")]
    [SerializeField] private CorridorEndTrigger leftCorridorEndTrigger;
    [SerializeField] private CorridorEndTrigger rightCorridorEndTrigger;

    [Header("플레이어 위치")]
    [SerializeField] private CharacterController playerController;
    [SerializeField] private Transform player;

    [Tooltip("통로 끝에 도착한 뒤 다음 방처럼 보이게 이동할 위치입니다.")]
    [SerializeField] private Transform nextRoomSpawnPoint;

    [Header("진행도 텍스트")]
    [SerializeField] private TMP_Text[] progressTexts;

    [Header("전환 효과")]
    [SerializeField] private FadeUI fadeUI;

    [Header("클리어 이벤트")]
    [SerializeField] private UnityEvent onStageClear;

    private int correctCount;
    private bool hasAnomaly;
    private bool isTransitioning;
    private bool isInCorridor;

    private int lastAnomalyIndex = -1;
    private AnomalyBase currentAnomaly;

    private void Start()
    {
        ResetDoors();
        ResetCorridorEnterTriggers();
        ResetCorridorEndTriggers();

        StartNewRound();
    }

    public void EnterCorridor(Stage2DoorType corridorType)
    {
        if (isTransitioning)
        {
            return;
        }

        isInCorridor = true;

        Debug.Log($"{corridorType} 통로 진입");
    }

    public void ReachCorridorEnd(Stage2DoorType selectedDoor)
    {
        if (isTransitioning)
        {
            return;
        }

        if (!isInCorridor)
        {
            return;
        }

        StartCoroutine(ResolveCorridorRoutine(selectedDoor));
    }

    private IEnumerator ResolveCorridorRoutine(Stage2DoorType selectedDoor)
    {
        isTransitioning = true;

        bool isCorrect = CheckAnswer(selectedDoor);

        if (isCorrect)
        {
            correctCount++;
            Debug.Log($"정답! {correctCount}/{clearRequiredCount}");
        }
        else
        {
            correctCount = 0;
            Debug.Log("오답! 진행도 초기화");
        }

        UpdateProgressText();

        if (correctCount >= clearRequiredCount)
        {
            yield return ClearStageRoutine();
            yield break;
        }

        if (fadeUI != null)
        {
            yield return fadeUI.FadeOut();
        }

        PrepareNextRoom();
        MovePlayer(nextRoomSpawnPoint);

        if (fadeUI != null)
        {
            yield return fadeUI.FadeIn();
        }

        isInCorridor = false;
        isTransitioning = false;
    }

    private bool CheckAnswer(Stage2DoorType selectedDoor)
    {
        if (hasAnomaly)
        {
            return selectedDoor == Stage2DoorType.Left;
        }

        return selectedDoor == Stage2DoorType.Right;
    }

    private void PrepareNextRoom()
    {
        ResetDoors();
        ResetCorridorEnterTriggers();
        ResetCorridorEndTriggers();
        StartNewRound();
    }

    private void StartNewRound()
    {
        ResetAllAnomalies();

        hasAnomaly = Random.value > noAnomalyChance;

        if (hasAnomaly)
        {
            ActivateRandomOneAnomaly();
        }
        else
        {
            currentAnomaly = null;
            Debug.Log("이번 방: 이상현상 없음");
        }

        UpdateProgressText();
    }

    private void ActivateRandomOneAnomaly()
    {
        if (anomalies == null || anomalies.Length == 0)
        {
            Debug.LogWarning("등록된 이상현상이 없습니다. 정상 방으로 처리합니다.");
            hasAnomaly = false;
            return;
        }

        int randomIndex = Random.Range(0, anomalies.Length);

        if (anomalies.Length > 1)
        {
            while (randomIndex == lastAnomalyIndex)
            {
                randomIndex = Random.Range(0, anomalies.Length);
            }
        }

        lastAnomalyIndex = randomIndex;
        currentAnomaly = anomalies[randomIndex];

        if (currentAnomaly == null)
        {
            Debug.LogWarning("선택된 이상현상이 비어 있습니다. 정상 방으로 처리합니다.");
            hasAnomaly = false;
            return;
        }

        currentAnomaly.ActivateAnomaly();

        Debug.Log("이번 방 이상현상: " + currentAnomaly.AnomalyName);
    }

    private void ResetAllAnomalies()
    {
        if (anomalies == null)
        {
            return;
        }

        for (int i = 0; i < anomalies.Length; i++)
        {
            if (anomalies[i] != null)
            {
                anomalies[i].ResetAnomaly();
            }
        }

        currentAnomaly = null;
    }

    private void ResetDoors()
    {
        if (leftDoor != null)
        {
            leftDoor.ResetDoor();
        }

        if (rightDoor != null)
        {
            rightDoor.ResetDoor();
        }
    }

    private void ResetCorridorEnterTriggers()
    {
        if (leftCorridorEnterTrigger != null)
        {
            leftCorridorEnterTrigger.ResetTrigger();
        }

        if (rightCorridorEnterTrigger != null)
        {
            rightCorridorEnterTrigger.ResetTrigger();
        }
    }

    private void ResetCorridorEndTriggers()
    {
        if (leftCorridorEndTrigger != null)
        {
            leftCorridorEndTrigger.ResetTrigger();
        }

        if (rightCorridorEndTrigger != null)
        {
            rightCorridorEndTrigger.ResetTrigger();
        }
    }

    private IEnumerator ClearStageRoutine()
    {
        isTransitioning = true;

        ResetAllAnomalies();
        UpdateProgressText();

        Debug.Log("2스테이지 클리어!");

        if (fadeUI != null)
        {
            yield return fadeUI.FadeOut();
        }

        onStageClear?.Invoke();

        if (fadeUI != null)
        {
            yield return fadeUI.FadeIn();
        }
    }

    private void MovePlayer(Transform targetPoint)
    {
        if (player == null || targetPoint == null)
        {
            return;
        }

        if (playerController != null)
        {
            playerController.enabled = false;
        }

        player.position = targetPoint.position;
        player.rotation = targetPoint.rotation;

        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }

    private void UpdateProgressText()
    {
        if (progressTexts == null)
        {
            return;
        }

        for (int i = 0; i < progressTexts.Length; i++)
        {
            if (progressTexts[i] != null)
            {
                progressTexts[i].text = $"{correctCount}/{clearRequiredCount}";
            }
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ReachCorridorEnd(Stage2DoorType.Left);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ReachCorridorEnd(Stage2DoorType.Right);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            correctCount = 0;
            StartNewRound();
            UpdateProgressText();
            Debug.Log("스테이지2 진행도 리셋");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            correctCount = clearRequiredCount - 1;
            UpdateProgressText();
            Debug.Log("다음 정답 시 클리어 상태");
        }
    }
#endif
}
