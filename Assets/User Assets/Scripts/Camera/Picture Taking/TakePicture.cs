using System.Collections;
using UnityEngine;

public class TakePicture : MonoBehaviour
{
    [SerializeField] private int w;
    [SerializeField] private int h;
    [SerializeField] private Camera textureCam;

    private RenderTexture renderTexture;

    private bool isCapturing;

    private void OnEnable()
    {
        GameEvents.OnPhotoInputPressed += TryCapture;
        if (!textureCam)
            textureCam = Camera.main;
    }

    private void OnDisable()
    {
        GameEvents.OnPhotoInputPressed -= TryCapture;
    }


    private void TryCapture()
    {
        if (isCapturing)
            return;

        StartCoroutine(CapturePhoto_Co());
    }

    private IEnumerator CapturePhoto_Co()
    {
        int width = w > 0 ? w : Screen.width /5;
        int height = h > 0 ? h : Screen.height /5;

        renderTexture = new RenderTexture(width, height, 24);

        isCapturing = true;

        GameEvents.RaisePhotoCaptureStarted();

        yield return new WaitForEndOfFrame();

        textureCam.targetTexture = renderTexture;
        textureCam.Render();
        textureCam.targetTexture = null;

        GameEvents.RaisePictureTaken(renderTexture);

        isCapturing = false;
    }
}