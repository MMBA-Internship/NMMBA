using UnityEngine;
using UnityEngine.InputSystem;

public class PhotoInput : MonoBehaviour
{
	[SerializeField] private KeyCode photoKey = KeyCode.Space; // ADDED

	public void TakePhoto(InputAction.CallbackContext ctx)
	{
		if (ctx.started)
			GameEvents.RaisePhotoInputPressed();
	}
}