using System;
using UnityEngine;

public static class GameEvents
{
	// Use Func<T> if you need to return as well

	public static Action<RenderTexture> OnPictureTaken;
}
