using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[CreateAssetMenu(fileName = "FishData", menuName = "Scriptable Objects/FishData")]
public class FishData : ScriptableObject
{
	public GameObject modelPrefab;
	public string reference;
	public int scoreAmount;
	public List<Spline> paths;
	public float swimSpeed;
}
