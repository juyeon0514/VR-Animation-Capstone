using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSceneTransition : MonoBehaviour
{
    public static StageSceneTransition Instance { get; private set; }

    [Header("Fade")]
    [SerializeField] private FadeUI fadeUI;

    [Header("Scene Names")]
    [SerializeField] private string startSceneName = "MainHall";
    [SerializeField] private string stage1SceneName = "FirstScene";
    [SerializeField] private string stage2SceneName = "SecondScene";
    [SerializeField] private string endingSceneName = "EndingScene";

    private bool isTransitioning;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator Start()
    {
        if (fadeUI != null)
        {
            fadeUI.SetBlack();
            yield return fadeUI.FadeIn();
        }
    }

    public void GoToStartScene()
    {
        LoadScene(startSceneName);
    }

    public void GoToStage1()
    {
        LoadScene(stage1SceneName);
    }

    public void GoToStage2()
    {
        LoadScene(stage2SceneName);
    }

    public void GoToEnding()
    {
        LoadScene(endingSceneName);
    }

    public void LoadScene(string sceneName)
    {
        if (isTransitioning)
        {
            return;
        }

        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        isTransitioning = true;

        if (fadeUI != null)
        {
            yield return fadeUI.FadeOut();
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (fadeUI != null)
        {
            yield return fadeUI.FadeIn();
        }

        isTransitioning = false;
    }

    public void LoadSceneFromBlack(string sceneName)
    {
        if (isTransitioning)
        {
            return;
        }

        StartCoroutine(LoadSceneFromBlackRoutine(sceneName));
    }

    private IEnumerator LoadSceneFromBlackRoutine(string sceneName)
    {
        isTransitioning = true;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (fadeUI != null)
        {
            yield return fadeUI.FadeIn();
        }

        isTransitioning = false;
    }
}
