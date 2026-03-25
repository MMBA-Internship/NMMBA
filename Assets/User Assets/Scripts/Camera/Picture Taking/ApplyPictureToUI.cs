using UnityEngine;
using UnityEngine.UI;

public class ApplyPictureToUI : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;

    private void OnEnable()
    {
        GameEvents.OnPictureTaken += SetPicture;
    }

    private void OnDisable()
    {
        GameEvents.OnPictureTaken -= SetPicture;
    }

    private void SetPicture(RenderTexture rt)
    {
        rawImage.color = Color.white;
        rawImage.texture = rt;
    }
}
