using UnityEngine;
using UnityEngine.UI;

public class ApplyPictureToUI : MonoBehaviour
{
    [SerializeField] private RenderTextureValue renderTextureValue;
    [SerializeField] private EventHolder onPictureTaken;

    [SerializeField] private RawImage rawImage;

    private void OnEnable()
    {
        onPictureTaken.Subscribe(SetPicture);
    }

    private void OnDisable()
    {
        onPictureTaken.Unsubscribe(SetPicture);
    }

    private void SetPicture()
    {
        rawImage.color = Color.white;
        rawImage.texture = renderTextureValue.value;
    }
}
