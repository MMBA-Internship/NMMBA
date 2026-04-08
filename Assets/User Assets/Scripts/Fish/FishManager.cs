using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(FishData), typeof(Animator))]
public class FishManager : MonoBehaviour
{
	[SerializeField] private float speedMultiplier = 3.5f;
	private float originalSpeed;

	[SerializeField] private List<Spline> splines;
	private SplineAnimate splineAnimate;
	private Coroutine speedRoutine;

	//[SerializeField] private AnimationClip scareClip;
	[SerializeField] private string triggerName;
	private Animator animator;

	private void OnEnable()
	{
		animator = GetComponent<Animator>();
		splineAnimate = GetComponent<SplineAnimate>();
		if (splineAnimate)
			originalSpeed = splineAnimate.MaxSpeed;
	}

	public void Scare()
	{
		if (animator && !string.IsNullOrEmpty(triggerName))
		{
			Debug.Log($"playing anim: {triggerName}");
			animator.SetTrigger(triggerName);
		}

		if (splineAnimate)
		{
			if (speedRoutine != null)
				StopCoroutine(speedRoutine);

			float boostedSpeed = originalSpeed * speedMultiplier;

			speedRoutine = StartCoroutine(EaseSpeed(boostedSpeed, 0.3f));

			Invoke(nameof(RevertSpeed), 3f);
		}
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
