using System.Collections;
using UnityEngine;

public class TakePicture : MonoBehaviour
{
    [SerializeField] private int w;
    [SerializeField] private int h;

    private RenderTexture renderTexture;

    private bool isCapturing;

    private void OnEnable()
    {
        GameEvents.OnPhotoInputPressed += TryCapture;
    }

    private void OnDisable()
    {
        GameEvents.OnPhotoInputPressed -= TryCapture;
    }

    private void Start()
    {
        int width = w > 0 ? w : Screen.width / 10;
        int height = h > 0 ? h : Screen.height / 10;

        renderTexture = new RenderTexture(width, height, 24);
    }

    private void TryCapture()
    {
        if (isCapturing)
            return;

        StartCoroutine(CapturePhoto_Co());
    }

    private IEnumerator CapturePhoto_Co()
    {
        isCapturing = true;

        GameEvents.RaisePhotoCaptureStarted();

        yield return new WaitForEndOfFrame();

        Camera.main.targetTexture = renderTexture;
        Camera.main.Render();
        Camera.main.targetTexture = null;

        GameEvents.RaisePictureTaken(renderTexture);

        isCapturing = false;
    }
}