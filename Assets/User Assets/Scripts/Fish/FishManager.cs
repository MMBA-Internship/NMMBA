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

	private void OnEnable()
	{
		fishData = GetComponent<FishData>();
		splineAnimate = GetComponent<SplineAnimate>();
		if (splineAnimate)
			originalSpeed = splineAnimate.MaxSpeed;
	}

	public void Scare()
	{
		if (splineAnimate)
		{
			UpdatePathSpeed(splineAnimate.MaxSpeed * speedMultiplier);
			Invoke("RevertSpeed", 3f);
		}
		Debug.Log($"I'm scared {fishData.reference}");
		/*	TODO:
		 * Switch spline
		 * Match point on spline based on time passed
		 * Manage speed (ease it!)
		 * Play animation
		*/
	}

	private void RevertSpeed()
	{
		UpdatePathSpeed(originalSpeed);
	}

	private void UpdatePathSpeed(float newSpeed)
	{
		float prevProgress = splineAnimate.NormalizedTime;
		splineAnimate.MaxSpeed = newSpeed;
		splineAnimate.NormalizedTime = prevProgress;
	}
}
