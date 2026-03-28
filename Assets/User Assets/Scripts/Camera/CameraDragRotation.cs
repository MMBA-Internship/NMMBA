using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CameraDragRotation : MonoBehaviour
{
    [Header("Input")]
    public InputAction point;
    public InputAction delta;
    public InputAction press;

    [Header("Camera")]
    public Transform cameraPivot;
    public float sensitivity = 0.2f;
    public float minPitch = -60f;
    public float maxPitch = 60f;
    public bool ignoreUI = true;

    private float yaw;
    private float pitch;

    private void OnEnable()
    {
        point.Enable();
        delta.Enable();
        press.Enable();
    }

    private void OnDisable()
    {
        point.Disable();
        delta.Disable();
        press.Disable();
    }

    private void Start()
    {
        Vector3 angles = cameraPivot.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        if (pitch > 180f) pitch -= 360f;
    }

    private void Update()
    {
        // Only rotate while dragging
        if (!press.IsPressed())
            return;

        Vector2 d = delta.ReadValue<Vector2>();

        // Optional deadzone (prevents jitter)
        if (d.sqrMagnitude < 0.01f)
            return;

        yaw += d.x * sensitivity;
        pitch -= d.y * sensitivity;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        cameraPivot.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}