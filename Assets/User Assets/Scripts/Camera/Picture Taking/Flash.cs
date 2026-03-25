using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float flashAlpha = 0.4f; // CHANGED: actually used now
    [SerializeField] private float fadeSpeed = 2.0f;  // CHANGED: actually used now

    private void OnEnable()
    {
<<<<<<< Updated upstream:Assets/User Assets/Scripts/Camera/Picture Taking/Flash.cs
        GameEvents.OnPictureTaken += FlashScreen;
=======
        // CHANGED: flash reacts to a valid capture start, not raw button input
        GameEvents.OnPhotoCaptureStarted += TriggerFlash;
>>>>>>> Stashed changes:Assets/User Assets/Scripts/Picture taking/Picture taking scripts/Flash.cs
    }

    private void OnDisable()
    {
<<<<<<< Updated upstream:Assets/User Assets/Scripts/Camera/Picture Taking/Flash.cs
        GameEvents.OnPictureTaken -= FlashScreen;
    }

    void FlashScreen(RenderTexture rt)
=======
        GameEvents.OnPhotoCaptureStarted -= TriggerFlash;
    }

    // CHANGED
    private void TriggerFlash()
>>>>>>> Stashed changes:Assets/User Assets/Scripts/Picture taking/Picture taking scripts/Flash.cs
    {
        Color color = image.color;
        color.a = flashAlpha;
        image.color = color;
    }

    // CHANGED: UI fade should use Update, not FixedUpdate
    private void Update()
    {
        if (image.color.a <= 0f)
            return;

        Color color = image.color;
        color.a -= fadeSpeed * Time.deltaTime;
        color.a = Mathf.Max(color.a, 0f);
        image.color = color;
    }
}