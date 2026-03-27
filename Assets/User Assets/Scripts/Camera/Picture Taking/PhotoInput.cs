using UnityEngine;
using UnityEngine.InputSystem;

public class PhotoInput : MonoBehaviour
{
	[SerializeField] private KeyCode photoKey = KeyCode.Space; // ADDED

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
		if (ctx.started && !disableInput)
			GameEvents.RaisePhotoInputPressed();
	}
}