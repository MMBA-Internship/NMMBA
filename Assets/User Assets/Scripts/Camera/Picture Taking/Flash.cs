using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float cooldown;

    private void OnEnable()
    {
        GameEvents.OnPictureTaken += FlashScreen;
    }

    private void OnDisable()
    {
        GameEvents.OnPictureTaken -= FlashScreen;
    }

    void FlashScreen(RenderTexture rt)
    {
        Color color = image.color;
        color.a = 0.4f;
        image.color = color;
    }

    private void FixedUpdate()
    {
        if (image.color.a > 0)
        {
            Color color = image.color;
            color.a -= 0.01f;

            image.color = color;
        }
    }
}
