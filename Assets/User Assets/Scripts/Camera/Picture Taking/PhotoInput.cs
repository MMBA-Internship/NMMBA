using UnityEngine;

public class PhotoInput : MonoBehaviour
{
    [SerializeField] private KeyCode photoKey = KeyCode.Space; // ADDED

    private void Update()
    {
        if (Input.GetKeyDown(photoKey))
        {
            GameEvents.RaisePhotoInputPressed(); // ADDED
        }
    }
}