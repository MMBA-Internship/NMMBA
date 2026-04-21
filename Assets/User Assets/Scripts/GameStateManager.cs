using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameStateManager : MonoBehaviour
{
	public bool WallBuild = false;

	[Header("UI Screens")]
	public GameObject ConfigScreen;
	public GameObject LobbyScreen;
	public GameObject GameplayScreen_A;
	public GameObject GameplayScreen_B;
	public GameObject Tutorial_Objective;
	public GameObject Tutorial_Controls;
    public GameObject Controls_A;
    public GameObject Controls_B;
    public GameObject RoundOver;
	public GameObject EndScore;
	public GameObject Gallery;

	[Header("Config UI Elements")]
	public GameObject HandDetectedUI;
	public TMP_InputField MaxCameraDepth;
	public TMP_InputField MinCameraDepth;
	public TMP_InputField CameraRegionStart;
	public TMP_InputField CameraRegionEnd;
	public TextMeshProUGUI CameraErrorText;

	[Header("Lobby UI Elements")]
	public GameObject notReadyState;
	public GameObject readyState;
	public GameObject tutorialButton;
	public GameObject exitTutorial;
	public TextMeshProUGUI countdownText;

	[Header("Gameplay UI Elements")]
	public TextMeshProUGUI oxygenTimerText_A;
	public TextMeshProUGUI oxygenTimerText_B;
	public TextMeshProUGUI finalScoreText;

	[Header("Settings")]
	public float countdownDuration = 10f;
	public float oxygenDuration = 180f;
	public float roundOverDuration = 5f;

	[Header("ScoreSaving")]
	public SaveLoad saveLoadManager;

	private List<int> scores;
    private int currentScore = 0;
	private bool useVersionA = true;
	private Coroutine countdownCoroutine;

	private bool isChanging = false;

	void Start()
	{

        if (WallBuild)
        {
            ShowConfig();
        }
        else
        {
            ShowLobby();
        }

    }

	void OnEnable()
	{
		GameEvents.OnSessionScoreChanged += UpdateScore;
		GameEvents.OnHandEntered += ShowHandUI;
		GameEvents.OnHandExited += HideHandUI;
		GameEvents.On3DCameraConnectionError += ShowCameraError;
	}


	void OnDisable()
	{
		GameEvents.OnSessionScoreChanged -= UpdateScore;
		GameEvents.OnHandEntered -= ShowHandUI;
		GameEvents.OnHandExited -= HideHandUI;
		GameEvents.On3DCameraConnectionError -= ShowCameraError;
	}

	private void ShowCameraError(Exception exception)
	{
		CameraErrorText.gameObject.SetActive(true);
		CameraErrorText.text = "Error: " + exception.Message;

	}

	private void HideHandUI()
	{
		HandDetectedUI.SetActive(false);
	}

	private void ShowHandUI()
	{
		HandDetectedUI.SetActive(true);
	}

	public void Try3DCameraConnect()
	{
		CameraErrorText.gameObject.SetActive(false);
		GameEvents.RaiseTry3DCameraConnect();
	}

    public void SelectVersionA()
    {
        useVersionA = true;
        GameEvents.RaiseControlVersionChanged(useVersionA);
        UpdateVersionDisplay();
        UpdateGameplayScreen();
    }

    public void SelectVersionB()
    {
        useVersionA = false;
        GameEvents.RaiseControlVersionChanged(useVersionA);
        UpdateVersionDisplay();
        UpdateGameplayScreen();
    }
    private void UpdateGameplayScreen()
    {
        bool inGameplay = GameplayScreen_A.activeSelf || GameplayScreen_B.activeSelf;
        if (!inGameplay) return;

        if (useVersionA)
        {
            GameplayScreen_A.SetActive(true);
            GameplayScreen_B.SetActive(false);
        }
        else
        {
            GameplayScreen_A.SetActive(false);
            GameplayScreen_B.SetActive(true);
        }
    }
    private void UpdateVersionDisplay()
	{
        //show which control is selected
        Controls_A.SetActive(useVersionA);
        Controls_B.SetActive(!useVersionA);

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
		Tutorial_Objective.SetActive(false);
		Tutorial_Controls.SetActive(false);
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

    public void OnTutorialObjectivePressed()
    {
        Tutorial_Objective.SetActive(true);
        Tutorial_Controls.SetActive(false);
    }

    public void OnTutorialContinue()
    {
        Tutorial_Objective.SetActive(false);
        Tutorial_Controls.SetActive(true);
    }
    public void OnTutorialExit()
	{
		Tutorial_Objective.SetActive(false);
		Tutorial_Controls.SetActive(false);
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

	public void StartGameplay()
	{
        //hide lobby, show gameplay
        LobbyScreen.SetActive(false);
		ConfigScreen.SetActive(false);

		if (useVersionA && !WallBuild)
		{
			GameplayScreen_A.SetActive(true);
		}
		else if (!WallBuild)
		{
			GameplayScreen_B.SetActive(true);
		}

		StartCoroutine(GameplayFlow());
	}

	IEnumerator GameplayFlow()
	{
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

			if (!WallBuild)
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
		int score = GameEvents.RaiseScoreRequested();
		Debug.Log(score);
		RoundOver.SetActive(false);
		EndScore.SetActive(true);
		finalScoreText.text = currentScore.ToString();
		saveLoadManager.score = currentScore;
		saveLoadManager.SaveData();
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

