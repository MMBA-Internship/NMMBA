using UnityEngine;
using UnityEngine.UI;

public class ApplyPictureToUI : MonoBehaviour
{
    [SerializeField] private RawImage photoDisplay;

    private void OnEnable()
    {
        GameEvents.OnPictureTaken += ShowPictureOnUI;
    }

    private void OnDisable()
    {
        GameEvents.OnPictureTaken -= ShowPictureOnUI;
    }

    private void ShowPictureOnUI(RenderTexture picture)
    {
        photoDisplay.texture = picture;
    }
}