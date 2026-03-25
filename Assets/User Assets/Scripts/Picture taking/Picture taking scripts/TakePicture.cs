using System.Collections;
using UnityEngine;

public class TakePicture : MonoBehaviour
{
    [SerializeField] private RenderTextureValue renderTextureValue;
    [SerializeField] private Texture2DValue texture2DValue;

    [Header("Capture Settings")]
    [SerializeField] private Camera captureCamera; // ADDED
    [SerializeField] private float captureCooldown = 1.0f; // ADDED

    private bool isCapturing; // ADDED
    private float lastCaptureTime = -999f; // ADDED

    private void Awake()
    {
        // ADDED: fallback in case not assigned in inspector
        if (captureCamera == null)
        {
            captureCamera = Camera.main;
        }
    }

    private void OnEnable()
    {
        // CHANGED: now listens to bus instead of direct calls / old event setup
        GameEvents.OnPhotoInputPressed += TryCapture;
    }

    private void OnDisable()
    {
        GameEvents.OnPhotoInputPressed -= TryCapture;
    }

    private void Start()
    {
        // EXISTING IDEA KEPT: create texture storage once
        texture2DValue.value = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        // ADDED: create render texture once if missing
        if (renderTextureValue.value == null)
        {
            renderTextureValue.value = new RenderTexture(Screen.width, Screen.height, 24);
        }
    }

    // ADDED
    private void TryCapture()
    {
        if (isCapturing)
            return;

        if (Time.time < lastCaptureTime + captureCooldown)
            return;

        StartCoroutine(CapturePhoto_Co());
    }

    private IEnumerator CapturePhoto_Co()
    {
        isCapturing = true;
        lastCaptureTime = Time.time;

        // ADDED: notify valid capture start
        GameEvents.RaisePhotoCaptureStarted();

        yield return new WaitForEndOfFrame();

        captureCamera.targetTexture = renderTextureValue.value;
        captureCamera.Render();
        captureCamera.targetTexture = null;

        // OPTIONAL BUT USEFUL: copy RenderTexture into Texture2D too
        // ADDED
        RenderTexture currentActive = RenderTexture.active;
        RenderTexture.active = renderTextureValue.value;

        texture2DValue.value.ReadPixels(
            new Rect(0, 0, renderTextureValue.value.width, renderTextureValue.value.height),
            0,
            0
        );
        texture2DValue.value.Apply();

        RenderTexture.active = currentActive;

        // CHANGED: bus event instead of old event holder
        GameEvents.RaisePictureTaken(renderTextureValue.value);

        isCapturing = false;
    }
}