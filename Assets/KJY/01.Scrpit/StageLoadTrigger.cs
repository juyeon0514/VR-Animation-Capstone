using UnityEngine;

public class StageLoadTrigger : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string nextSceneName = "SecondScene";

    private bool used;

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

        if (StageSceneTransition.Instance != null)
        {
            StageSceneTransition.Instance.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("StageSceneTransition.Instance가 없습니다. 시작씬에 SceneTransitionManager가 있는지 확인하세요.");
        }
    }
}
