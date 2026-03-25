using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float flashAlpha = 0.4f; // CHANGED: actually used now
    [SerializeField] private float fadeSpeed = 2.0f;  // CHANGED: actually used now

    private void OnEnable()
    {
        // CHANGED: flash reacts to a valid capture start, not raw button input
        GameEvents.OnPhotoCaptureStarted += TriggerFlash;
    }

    private void OnDisable()
    {
        GameEvents.OnPhotoCaptureStarted -= TriggerFlash;
    }

    // CHANGED
    private void TriggerFlash()
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