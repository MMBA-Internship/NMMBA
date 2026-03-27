using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhotoInput : MonoBehaviour
{
    [SerializeField] private KeyCode photoKey = KeyCode.Space;
    [SerializeField] private float photoCooldown = 3f;
    bool disableInput = false;

    private void OnEnable()
    {
        GameEvents.OnRoundEnded += OnRoundEnded;
    }

    private void OnDisable()
    {
        GameEvents.OnRoundEnded -= OnRoundEnded;
    }

    private void OnRoundEnded()
    {
        disableInput = true;
    }

    public void TakePhoto(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            TakePhoto();
        }
    }

    private IEnumerator PhotoCooldown_Co()
    {

        disableInput = true;
        yield return new WaitForSeconds(photoCooldown);
        disableInput = false;
    }

    public void TakePhoto()
    {
        Debug.Log("PhotoInput: TakePhoto called");
        if (!disableInput)
        {
            Debug.Log("PhotoInput: TakePhoto called");
            GameEvents.RaisePhotoInputPressed();
            StartCoroutine(PhotoCooldown_Co()); ;
        }
        Debug.Log("PhotoInput: TakePhoto finished");
    }


    public void debug()
    {
        Debug.Log("PhotoInput: debug method called");
    }

}