using System;
using UnityEngine;

public static class GameEvents
{
	/// <summary>
	/// Fires when something enters the vision of the 3D camera
	/// </summary>
	public static event Action OnHandEntered;
	public static void RaiseHandEntered() => OnHandEntered?.Invoke();



	/// <summary>
	/// Fires when the 3D camera no longer detects anything in it's cone of visibility.
	/// </summary>
	public static event Action OnHandExited;
	public static void RaiseHandExited() => OnHandExited?.Invoke();



	/// <summary>
	/// Fires when settings are updated and saved from the config menu of the wall
	/// </summary>
	public static event Action<float, float, float, float> On3DCameraSettingsSaved;
	public static void Raise3DCameraSettingsSaved(
		float maxDepth,
		float minDepth,
		float regionStart,
		float regionEnd) => On3DCameraSettingsSaved?.Invoke(maxDepth, minDepth, regionStart, regionEnd);



	/// <summary>
	/// Fires when 3D Realsense camera throws an exception
	/// </summary>
	public static event Action<Exception> On3DCameraConnectionError;
	public static void Raise3DCameraConnectionError(Exception e) => On3DCameraConnectionError?.Invoke(e);



	public static event Action OnTry3DCameraConnect;
	public static void RaiseTry3DCameraConnect() => OnTry3DCameraConnect?.Invoke();



	/// <summary>
	/// Input asks for a photo through the bus
	/// </summary>
	public static event Action OnPhotoInputPressed;
	public static void RaisePhotoInputPressed() => OnPhotoInputPressed?.Invoke();



	/// <summary>
	/// Fires when the photo cooldown is over and the player can take another photo
	/// </summary>
	public static event Action OnPhotoCooldownEnded;
	public static void RaisePhotoCooldownEnded() => OnPhotoCooldownEnded?.Invoke();


	/// <summary>
	/// Fires when a photo capture is actually accepted,
	/// flash is listening
	/// </summary>
	public static event Action OnPhotoCaptureStarted;
	public static void RaisePhotoCaptureStarted() => OnPhotoCaptureStarted?.Invoke();



	/// <summary>
	/// Fires when the render texture is ready
	/// </summary>
	public static event Action<RenderTexture> OnPictureTaken;
	public static void RaisePictureTaken(RenderTexture picture) => OnPictureTaken?.Invoke(picture);



	/// <summary>
	/// Fires after score calculation for one picture
	/// </summary>
	public static event Action<SinglePhotoScoreResult> OnPhotoScored;
	public static void RaisePhotoScored(SinglePhotoScoreResult result) => OnPhotoScored?.Invoke(result);



	/// <summary>
	/// Fired whenever total session score changes
	/// </summary>
	public static event Action<int> OnSessionScoreChanged;
	public static void RaiseSessionScoreChanged(int totalScore) => OnSessionScoreChanged?.Invoke(totalScore);



	/// <summary>
	/// request score from the score manager
	/// </summary>
	public static event Func<int> OnScoreRequested;
	public static int RaiseScoreRequested() => OnScoreRequested?.Invoke() ?? 0;



	/// <summary>
	/// Scene flow
	/// </summary>
	public static event Action<string> OnSceneChangeRequested;
	public static void RaiseSceneChangeRequested(string sceneName) => OnSceneChangeRequested?.Invoke(sceneName);



	public static event Action<string, float> OnSceneChangeRequestedAfterCountdown;
	public static void RaiseSceneChangeRequestedAfterCountdown(string sceneName, float countdownSeconds)
		=> OnSceneChangeRequestedAfterCountdown?.Invoke(sceneName, countdownSeconds);



	public static event Action<float> OnSceneCountdownUpdated;
	public static void RaiseSceneCountdownUpdated(float timeRemaining)
		=> OnSceneCountdownUpdated?.Invoke(timeRemaining);



	/// <summary>
	/// Fires when the timer runs out
	/// </summary>
	public static event Action OnRoundEnded;
	public static void RaiseRoundEnded() => OnRoundEnded?.Invoke();



    /// <summary>
    /// Fires when the control version is changed. True for version A, false for version B.
    /// </summary>
    public static event Action<bool> OnControlVersionChanged;
    public static void RaiseControlVersionChanged(bool versionA) => OnControlVersionChanged?.Invoke(versionA);


    /// <summary>
    /// Fires when Gallery Screen gets activated
    /// </summary>
    public static event Action OnGalleryScreenActivated;
    public static void RaiseGalleryScreenActivated() => OnGalleryScreenActivated?.Invoke();

}