using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlowManager : MonoBehaviour
{
    // scene names have to be EXACTLY the same as in the build settings or the scene loading fails
    [SerializeField] private string menuScene = "Menu";
    [SerializeField] private string lobbyScene = "Lobby";
    [SerializeField] private string gameScene = "Game";
    [SerializeField] private string resultsScene = "Results";

    public float CountdownTimeRemaining { get; private set; }

    private Coroutine countdownRoutine;

    private void Awake()
    {
        // DontDestroyOnLoad keeps this manager alive when scenes change
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        // this listens to the GameEvents bus so other scripts can request scene changes without talking to SceneManager directly
        GameEvents.OnSceneChangeRequested += ChangeSceneNow;
        GameEvents.OnSceneChangeRequestedAfterCountdown += ChangeSceneAfterCountdown;
    }

    private void OnDisable()
    {
        GameEvents.OnSceneChangeRequested -= ChangeSceneNow;
        GameEvents.OnSceneChangeRequestedAfterCountdown -= ChangeSceneAfterCountdown;
    }

    public void GoToMenu()
    {
        ChangeSceneNow(menuScene);
    }

    public void GoToLobby()
    {
        ChangeSceneNow(lobbyScene);
    }

    public void GoToGame()
    {
        ChangeSceneNow(gameScene);
    }

    public void GoToResults()
    {
        ChangeSceneNow(resultsScene);
    }

    private void ChangeSceneNow(string sceneName)
    {
        // this one is just for instant scene switching, no countdown or extra stuff
        if (string.IsNullOrWhiteSpace(sceneName))
            return;

        // if another countdown starts while one is already running, the old one gets stopped first
        if (countdownRoutine != null)
        {
            StopCoroutine(countdownRoutine);
            countdownRoutine = null;
        }

        CountdownTimeRemaining = 0f;

        // countdown time gets exposed through the event so UI can listen to it and show the number if needed
        GameEvents.RaiseSceneCountdownUpdated(CountdownTimeRemaining);

        SceneManager.LoadScene(sceneName);
    }

    private void ChangeSceneAfterCountdown(string sceneName, float countdownSeconds)
    {
        // this starts a countdown first and then swaps to the target scene when the timer hits 0
        if (string.IsNullOrWhiteSpace(sceneName))
            return;

        // setting countdown to 0 just skips straight to the next scene
        if (countdownSeconds <= 0f)
        {
            ChangeSceneNow(sceneName);
            return;
        }

        // if another countdown starts while one is already running, the old one gets stopped first
        if (countdownRoutine != null)
            StopCoroutine(countdownRoutine);

        countdownRoutine = StartCoroutine(CountdownRoutine(sceneName, countdownSeconds));
    }

    private IEnumerator CountdownRoutine(string sceneName, float countdownSeconds)
    {
        CountdownTimeRemaining = countdownSeconds;

        while (CountdownTimeRemaining > 0f)
        {
            // countdown time gets exposed through the event so UI can listen to it and show the number if needed
            GameEvents.RaiseSceneCountdownUpdated(CountdownTimeRemaining);

            CountdownTimeRemaining -= Time.deltaTime;
            yield return null;
        }

        CountdownTimeRemaining = 0f;
        GameEvents.RaiseSceneCountdownUpdated(CountdownTimeRemaining);

        countdownRoutine = null;
        SceneManager.LoadScene(sceneName);
    }
}