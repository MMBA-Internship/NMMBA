using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TakePicture : MonoBehaviour
{
	[SerializeField] private int w;
	[SerializeField] private int h;
	[SerializeField] private Texture2DValue texture2DValue;
	//[SerializeField] private RenderTextureValue renderTextureValue;
	private RenderTexture renderTexture;
	[SerializeField] private Image flashImage;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		texture2DValue.value = new Texture2D(Screen.width / 10, Screen.width / 10, TextureFormat.RGB24, false);
		renderTexture = new RenderTexture(Screen.width / 10, Screen.height / 10, 24);
	}

	IEnumerator CapturePhoto_Co()
	{
		yield return new WaitForEndOfFrame();

		Camera.main.targetTexture = renderTexture;

		Camera.main.Render();

		Camera.main.targetTexture = null;

		GameEvents.OnPictureTaken.Invoke(renderTexture);
	}

	public void Capture()
	{
		if (flashImage.color.a <= 0)
			StartCoroutine(CapturePhoto_Co());
	}
}
