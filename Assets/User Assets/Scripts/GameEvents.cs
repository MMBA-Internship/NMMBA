using System;
using UnityEngine;

public static class GameEvents
{
    // input asks for a photo through the bus
    public static event Action OnPhotoInputPressed;

    // when a photo capture is actually accepted
    // flash is listening
    public static event Action OnPhotoCaptureStarted;

    // when the render texture is ready
    public static event Action<RenderTexture> OnPictureTaken;

    // after score calculation for one picture
    public static event Action<SinglePhotoScoreResult> OnPhotoScored;

    // fired whenever total session score changes
    public static event Action<int> OnSessionScoreChanged;

    // request score
    public static event Func<int> OnScoreRequested;

    // scene flow
    public static event Action<string> OnSceneChangeRequested;
    public static event Action<string, float> OnSceneChangeRequestedAfterCountdown;
    public static event Action<float> OnSceneCountdownUpdated;

    public static void RaisePhotoInputPressed() => OnPhotoInputPressed?.Invoke();
    public static void RaisePhotoCaptureStarted() => OnPhotoCaptureStarted?.Invoke();
    public static void RaisePictureTaken(RenderTexture picture) => OnPictureTaken?.Invoke(picture);
    public static void RaisePhotoScored(SinglePhotoScoreResult result) => OnPhotoScored?.Invoke(result);
    public static void RaiseSessionScoreChanged(int totalScore) => OnSessionScoreChanged?.Invoke(totalScore);

    public static void RaiseSceneChangeRequested(string sceneName) => OnSceneChangeRequested?.Invoke(sceneName);

    public static void RaiseSceneChangeRequestedAfterCountdown(string sceneName, float countdownSeconds)
        => OnSceneChangeRequestedAfterCountdown?.Invoke(sceneName, countdownSeconds);

    public static void RaiseSceneCountdownUpdated(float timeRemaining)
        => OnSceneCountdownUpdated?.Invoke(timeRemaining);

    public static int RaiseScoreRequest() => OnScoreRequested?.Invoke() ?? 0;
}