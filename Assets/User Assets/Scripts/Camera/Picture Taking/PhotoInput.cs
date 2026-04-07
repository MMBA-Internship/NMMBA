using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhotoInput : MonoBehaviour
{
    [SerializeField] private float photoCooldown = 3f;
    [SerializeField] private InputAction doubleTap;
    bool disableInput = false;
    bool versionA = true;

    private void OnEnable()
    {
        GameEvents.OnRoundEnded += OnRoundEnded;
        GameEvents.OnControlVersionChanged += UpdateControls;
        doubleTap.Enable();
        doubleTap.performed += TakePhoto;
    }

	private void OnDisable()
    {
        GameEvents.OnRoundEnded -= OnRoundEnded;
        GameEvents.OnControlVersionChanged -= UpdateControls;
        doubleTap.performed -= TakePhoto;
        doubleTap.Disable();
    }

	private void UpdateControls(bool versionA)
	{
		this.versionA = versionA;
	}

    private void OnRoundEnded()
    {
        disableInput = true;
    }

    public void TakePhoto(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !versionA)
        {
            Debug.Log("Double clicked");
            TakePhoto();
        }
    }

    private IEnumerator PhotoCooldown_Co()
    {
        disableInput = true;
        yield return new WaitForSeconds(photoCooldown);
        disableInput = false;
        GameEvents.RaisePhotoCooldownEnded();
    }

    public void TakePhoto()
    {
        if (!disableInput)
        {
            GameEvents.RaisePhotoInputPressed();
            StartCoroutine(PhotoCooldown_Co());
        }
    }
}