using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(FishData))]
public class FishManager : MonoBehaviour
{
	[SerializeField] private float speedMultiplier = 5f;
	private float originalSpeed;
	[SerializeField] private List<Spline> splines;
	[SerializeField] private SplineAnimate splineAnimate;
	private FishData fishData;
	private Coroutine speedRoutine;


	private void OnEnable()
	{
		fishData = GetComponent<FishData>();
		splineAnimate = GetComponent<SplineAnimate>();
		if (splineAnimate)
			originalSpeed = splineAnimate.MaxSpeed;
	}

	public void Scare()
	{
		if (!splineAnimate) return;

		if (speedRoutine != null)
			StopCoroutine(speedRoutine);

		float boostedSpeed = originalSpeed * speedMultiplier;

		speedRoutine = StartCoroutine(EaseSpeed(boostedSpeed, 0.3f));

		Invoke(nameof(RevertSpeed), 3f);
	}

	private void RevertSpeed()
	{
		if (speedRoutine != null)
			StopCoroutine(speedRoutine);

		speedRoutine = StartCoroutine(EaseSpeed(originalSpeed, 0.8f));
	}

	private IEnumerator EaseSpeed(float targetSpeed, float duration)
	{
		float startSpeed = splineAnimate.MaxSpeed;
		float time = 0f;

		while (time < duration)
		{
			time += Time.deltaTime;
			float t = time / duration;

			t = t * t * (3f - 2f * t);

			float newSpeed = Mathf.Lerp(startSpeed, targetSpeed, t);
			UpdatePathSpeed(newSpeed);

			yield return null;
		}

		UpdatePathSpeed(targetSpeed);
	}

	private void UpdatePathSpeed(float newSpeed)
	{
		float prevProgress = splineAnimate.NormalizedTime;
		splineAnimate.MaxSpeed = newSpeed;
		splineAnimate.NormalizedTime = prevProgress;
	}
}
