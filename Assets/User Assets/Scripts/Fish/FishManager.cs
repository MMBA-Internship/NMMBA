using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(FishData))]
public class FishManager : MonoBehaviour
{
	[SerializeField] private float speed = 1f;
	[SerializeField] private List<Spline> splines;
	[SerializeField] private SplineAnimate splineAnimate;
	private FishData fishData;

	private void OnEnable()
	{
		fishData = GetComponent<FishData>();
	}

	public void Scare()
	{
		Debug.Log($"I'm scared {fishData.reference}");
		/*	TODO:
		 * Switch spline
		 * Match point on spline based on time passed
		 * Manage speed
		*/
	}
}
