using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TakePicture : MonoBehaviour
{
    [SerializeField] private int w;
    [SerializeField] private int h;
    [SerializeField] private Texture2DValue texture2DValue;
    [SerializeField] private RenderTextureValue renderTextureValue;
    [SerializeField] private EventHolder onPictureTaken;
    [SerializeField] private Image flashImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        texture2DValue.value = new Texture2D(Screen.width / 10, Screen.width / 10, TextureFormat.RGB24, false);
    }

    IEnumerator CapturePhoto_Co()
    {
        yield return new WaitForEndOfFrame();

        if (renderTextureValue.value == null)
            renderTextureValue.value = new RenderTexture(Screen.width / 10, Screen.height / 10, 24);

        Camera.main.targetTexture = renderTextureValue.value;

        Camera.main.Render();

        Camera.main.targetTexture = null;

        onPictureTaken.Invoke();
    }

    public void Capture()
    {
        if (flashImage.color.a <= 0)
            StartCoroutine(CapturePhoto_Co());
    }
}
