using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;


public class GameStateManager : MonoBehaviour
{
	[Header("UI Screens")]
	public GameObject ConfigScreen;
	public GameObject LobbyScreen;
	public GameObject GameplayScreen_A;
	public GameObject GameplayScreen_B;
	public GameObject Tutorial_A;
	public GameObject Tutorial_B;
	public GameObject RoundOver;
	public GameObject EndScore;
	public GameObject Gallery;

	[Header("Config UI Elements")]
	public GameObject HandDetectedUI;
	public TMP_InputField MaxCameraDepth;
	public TMP_InputField MinCameraDepth;
	public TMP_InputField CameraRegionStart;
	public TMP_InputField CameraRegionEnd;

	[Header("Lobby UI Elements")]
	public GameObject notReadyState;
	public GameObject readyState;
	public GameObject tutorialButton;
	public GameObject exitTutorial;
	public TextMeshProUGUI countdownText;
	public TextMeshProUGUI versionText;

	[Header("Gameplay UI Elements")]
	public TextMeshProUGUI oxygenTimerText_A;
	public TextMeshProUGUI oxygenTimerText_B;
	public TextMeshProUGUI finalScoreText;

	[Header("Settings")]
	public float countdownDuration = 10f;
	public float tutorialDuration = 5f;
	public float oxygenDuration = 180f;
	public float roundOverDuration = 5f;

	private int currentScore = 0;
	private bool useVersionA = true;
	private Coroutine countdownCoroutine;

	private bool isChanging = false;

	void Start()
	{
		ShowConfig();
	}

	void OnEnable()
	{
		GameEvents.OnSessionScoreChanged += UpdateScore;
		GameEvents.OnHandEntered += ShowHandUI;
		GameEvents.OnHandExited += HideHandUI;
	}

	void OnDisable()
	{
		GameEvents.OnSessionScoreChanged -= UpdateScore;
		GameEvents.OnHandEntered -= ShowHandUI;
		GameEvents.OnHandExited -= HideHandUI;
	}

	private void HideHandUI()
	{
		HandDetectedUI.SetActive(false);
	}

	private void ShowHandUI()
	{
		HandDetectedUI.SetActive(true);
	}

	public void ToggleVersion()
	{
		useVersionA = !useVersionA;
		GameEvents.RaiseControlVersionChanged(useVersionA);
		UpdateVersionDisplay();
	}

	private void UpdateVersionDisplay()
	{
		if (versionText != null)
		{
			versionText.text = useVersionA ? "Version A" : "Version B";
		}
	}

	void ShowConfig()
	{
		// Assuming that everything else is disabled by default
		ConfigScreen.SetActive(true);
	}

	public void ShowLobby()
	{
		//show only the lobby, hiding everything else
		LobbyScreen.SetActive(true);
		ConfigScreen.SetActive(false);
		GameplayScreen_A.SetActive(false);
		GameplayScreen_B.SetActive(false);
		Tutorial_A.SetActive(false);
		Tutorial_B.SetActive(false);
		RoundOver.SetActive(false);
		EndScore.SetActive(false);

		notReadyState.SetActive(true);
		readyState.SetActive(false);
		countdownText.text = "";

		GameEvents.OnHandEntered -= ShowHandUI;
		GameEvents.OnHandExited -= HideHandUI;

		UpdateVersionDisplay();

	}

	public void SaveSettings()
	{
		float maxDepth;
		float minDepth;
		float regionStart;
		float regionEnd;

		float.TryParse(MaxCameraDepth.text, out maxDepth);
		float.TryParse(MinCameraDepth.text, out minDepth);
		float.TryParse(CameraRegionStart.text, out regionStart);
		float.TryParse(CameraRegionEnd.text, out regionEnd);

		GameEvents.Raise3DCameraSettingsSaved(maxDepth, minDepth, regionStart, regionEnd);
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

	//called by ready button
	public void OnReadyPressed()
	{
		notReadyState.SetActive(false);
		readyState.SetActive(true);
		countdownCoroutine = StartCoroutine(CountdownThenStart());
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
		//stop the countdown
		if (countdownCoroutine != null)
		{
			StopCoroutine(countdownCoroutine);
			countdownCoroutine = null;
		}

		//reset back to not ready state
		notReadyState.SetActive(true);
		readyState.SetActive(false);
		countdownText.text = "";
	}

	void StartGameplay()
	{
		//hide lobby, show gameplay
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
		//show tutorial for a couple of seconds
		yield return new WaitForSeconds(tutorialDuration);

		Tutorial_A.SetActive(false);
		Tutorial_B.SetActive(false);

		//run oxygen timer
		yield return StartCoroutine(OxygenTimer());

		//timer finished round ended
		EndRound();
	}

	IEnumerator OxygenTimer()
	{
		float timeRemaining = oxygenDuration;

		//picking version timer
		TextMeshProUGUI activeTimerText = useVersionA ? oxygenTimerText_A : oxygenTimerText_B;


		while (timeRemaining > 0)
		{
			int minutes = Mathf.FloorToInt(timeRemaining / 60);
			int seconds = Mathf.FloorToInt(timeRemaining % 60);
			activeTimerText.text = $"{minutes:00}:{seconds:00}";

			timeRemaining -= Time.deltaTime;
			yield return null;
		}
	}

	void EndRound()
	{
		GameEvents.RaiseRoundEnded();

		//hide gameplay, show round over
		GameplayScreen_A.SetActive(false);
		GameplayScreen_B.SetActive(false);
		RoundOver.SetActive(true);

		StartCoroutine(ShowScoreAfterDelay());
	}

	IEnumerator ShowScoreAfterDelay()
	{
		yield return new WaitForSeconds(roundOverDuration);
		Debug.Log("requesting score");
		int score = GameEvents.RaiseScoreRequested();
		Debug.Log(score);
		RoundOver.SetActive(false);
		EndScore.SetActive(true);
		finalScoreText.text = currentScore.ToString();
	}

	public void OnContinuePressed()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void OnGalleryPressed()
    {
        EndScore.SetActive(false);
        Gallery.SetActive(true);
        GameEvents.RaiseGalleryScreenActivated();
    }

	void UpdateScore(int newScore)
	{
		currentScore = newScore;
	}

	public void SetEnglish()
	{
		ChangeLanguageByCode("en");
	}

	public void SetTraditionalChinese()
	{
		ChangeLanguageByCode("zh-TW");
	}

	private void ChangeLanguageByCode(string localeCode)
	{
		if (isChanging) return;
		StartCoroutine(ChangeLanguageByCodeCoroutine(localeCode));
	}

	private IEnumerator ChangeLanguageByCodeCoroutine(string localeCode)
	{
		isChanging = true;
		yield return LocalizationSettings.InitializationOperation;

		Locale selectedLocale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);
		if (selectedLocale != null)
		{
			LocalizationSettings.SelectedLocale = selectedLocale;
		}

		isChanging = false;
	}
}

