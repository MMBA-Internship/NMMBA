using UnityEngine;
using TMPro;
using System.Collections;

public class GameStateManager : MonoBehaviour
{
    [Header("UI Screens")]
    public GameObject LobbyScreen;
    public GameObject GameplayScreen_A;
    public GameObject GameplayScreen_B;
    public GameObject Tutorial_A;
    public GameObject Tutorial_B;
    public GameObject RoundOver;
    public GameObject EndScore;

    [Header("Lobby UI Elements")]
    public GameObject notReadyState;
    public GameObject readyState;
    public GameObject tutorialButton;
    public GameObject exitTutorial;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI versionText; 

    [Header("Gameplay UI Elements")]
    public TextMeshProUGUI oxygenTimerText;
    public TextMeshProUGUI finalScoreText;

    [Header("Settings")]
    public float countdownDuration = 10f;
    public float tutorialDuration = 5f;
    public float oxygenDuration = 180f;
    public float roundOverDuration = 5f;

    private int currentScore = 0;
    private bool useVersionA = true;
    private Coroutine countdownCoroutine;

    void Start()
    {
        ShowLobby();
    }

    void OnEnable()
    {
        GameEvents.OnSessionScoreChanged += UpdateScore;
    }

    void OnDisable()
    {
        GameEvents.OnSessionScoreChanged -= UpdateScore;
    }

    public void ToggleVersion()
    {
        useVersionA = !useVersionA;
        UpdateVersionDisplay();
    }

    private void UpdateVersionDisplay()
    {
        if (versionText != null)
        {
            versionText.text = useVersionA ? "Version A" : "Version B";
        }
    }

    void ShowLobby()
    {
        // Show only lobby, hide everything else
        LobbyScreen.SetActive(true);
        GameplayScreen_A.SetActive(false);
        GameplayScreen_B.SetActive(false);
        Tutorial_A.SetActive(false);
        Tutorial_B.SetActive(false);
        RoundOver.SetActive(false);
        EndScore.SetActive(false);

        notReadyState.SetActive(true);
        readyState.SetActive(false);
        countdownText.text = "";

        UpdateVersionDisplay();

    }

    public void OnTutorialPressed()
    {
        if (useVersionA)
        {
            Tutorial_A.SetActive(true);
        }
        else
        {
            Tutorial_B.SetActive(true);
        }
    }

    public void OnTutorialExit()
    {
        Tutorial_A.SetActive(false);
        Tutorial_B.SetActive(false);
    }

    // Called by Ready button
    public void OnReadyPressed()
    {
        notReadyState.SetActive(false);
        readyState.SetActive(true);
        countdownCoroutine = StartCoroutine(CountdownThenStart()); // Store it
    }

    IEnumerator CountdownThenStart()
    {
        float timeRemaining = countdownDuration;

        while (timeRemaining > 0)
        {
            countdownText.text = Mathf.Ceil(timeRemaining).ToString();
            timeRemaining -= Time.deltaTime;
            yield return null;
        }

        StartGameplay();
    }

    public void OnCancelPressed()
    {
        // Stop the countdown if it's running
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }

        // Reset back to not ready state
        notReadyState.SetActive(true);
        readyState.SetActive(false);
        countdownText.text = "";
    }

    void StartGameplay()
    {
        // Hide lobby, show gameplay
        LobbyScreen.SetActive(false);

        if (useVersionA)
        {
            GameplayScreen_A.SetActive(true);
            Tutorial_A.SetActive(true);
        }
        else
        {
            GameplayScreen_B.SetActive(true);
            Tutorial_B.SetActive(true);
        }

        StartCoroutine(GameplayFlow());
    }

    IEnumerator GameplayFlow()
    {
        // Show tutorial for 5 seconds
        yield return new WaitForSeconds(tutorialDuration);

        Tutorial_A.SetActive(false);
        Tutorial_B.SetActive(false);

        // Run oxygen timer
        yield return StartCoroutine(OxygenTimer());

        // Timer finished, end round
        EndRound();
    }

    IEnumerator OxygenTimer()
    {
        float timeRemaining = oxygenDuration;

        while (timeRemaining > 0)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            oxygenTimerText.text = $"{minutes:00}:{seconds:00}";

            timeRemaining -= Time.deltaTime;
            yield return null;
        }
    }

    void EndRound()
    {
        // Hide gameplay, show round over
        GameplayScreen_A.SetActive(false);
        GameplayScreen_B.SetActive(false);
        RoundOver.SetActive(true);

        StartCoroutine(ShowScoreAfterDelay());
    }

    IEnumerator ShowScoreAfterDelay()
    {
        yield return new WaitForSeconds(roundOverDuration);

        RoundOver.SetActive(false);
        EndScore.SetActive(true);
        finalScoreText.text = currentScore.ToString();
    }

    // Called by Continue button
    public void OnContinuePressed()
    {
        currentScore = 0;
        ShowLobby();
    }

    void UpdateScore(int newScore)
    {
        currentScore = newScore;
    }
}