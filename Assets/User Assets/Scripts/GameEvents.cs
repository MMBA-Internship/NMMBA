using System;
using UnityEngine;

public static class GameEvents
{
    /// <summary>
    /// Input asks for a photo through the bus
    /// </summary>
    public static event Action OnPhotoInputPressed;
    public static void RaisePhotoInputPressed() => OnPhotoInputPressed?.Invoke();



    /// <summary>
    /// when a photo capture is actually accepted,
    /// flash is listening
    /// </summary>
    public static event Action OnPhotoCaptureStarted;
    public static void RaisePhotoCaptureStarted() => OnPhotoCaptureStarted?.Invoke();



    /// <summary>
    /// When the render texture is ready
    /// </summary>
    public static event Action<RenderTexture> OnPictureTaken;
    public static void RaisePictureTaken(RenderTexture picture) => OnPictureTaken?.Invoke(picture);



    /// <summary>
    /// After score calculation for one picture
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
}