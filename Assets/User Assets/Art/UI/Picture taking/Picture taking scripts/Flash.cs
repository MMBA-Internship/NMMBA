using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
    [SerializeField] private EventHolder onPictureTaken;
    [SerializeField] private Image image;
    [SerializeField] private float cooldown;

    private void OnEnable()
    {
        onPictureTaken.Subscribe(FlashScreen);
    }

    private void OnDisable()
    {
        onPictureTaken.Unsubscribe(FlashScreen);
    }

    void FlashScreen()
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
