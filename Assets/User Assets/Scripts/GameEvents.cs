using System;
using UnityEngine;

public static class GameEvents
{
    // input asks for a photo through the bus
    public static event Action OnPhotoInputPressed;
    public static void RaisePhotoInputPressed() => OnPhotoInputPressed?.Invoke();

    // when a photo capture is actually accepted
    // flash is listening
    public static event Action OnPhotoCaptureStarted;
    public static void RaisePhotoCaptureStarted() => OnPhotoCaptureStarted?.Invoke();

    // when the render texture is ready
    public static event Action<RenderTexture> OnPictureTaken;
    public static void RaisePictureTaken(RenderTexture picture) => OnPictureTaken?.Invoke(picture);

    // after score calculation for one picture
    public static event Action<SinglePhotoScoreResult> OnPhotoScored;
    public static void RaisePhotoScored(SinglePhotoScoreResult result) => OnPhotoScored?.Invoke(result);

    // fired whenever total session score changes
    public static event Action<int> OnSessionScoreChanged;
    public static void RaiseSessionScoreChanged(int totalScore) => OnSessionScoreChanged?.Invoke(totalScore);

    // request score
    public static event Func<int> OnScoreRequested;
    public static int RaiseScoreRequested() => OnScoreRequested?.Invoke() ?? 0;

    // scene flow
    public static event Action<string> OnSceneChangeRequested;
    public static void RaiseSceneChangeRequested(string sceneName) => OnSceneChangeRequested?.Invoke(sceneName);

    public static event Action<string, float> OnSceneChangeRequestedAfterCountdown;
    public static void RaiseSceneChangeRequestedAfterCountdown(string sceneName, float countdownSeconds)
        => OnSceneChangeRequestedAfterCountdown?.Invoke(sceneName, countdownSeconds);

    public static event Action<float> OnSceneCountdownUpdated;
    public static void RaiseSceneCountdownUpdated(float timeRemaining)
        => OnSceneCountdownUpdated?.Invoke(timeRemaining);

    public static event Action OnRoundEnded;
    public static void RaiseRoundEnded() => OnRoundEnded?.Invoke();
}