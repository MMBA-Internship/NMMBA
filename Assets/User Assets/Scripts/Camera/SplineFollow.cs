using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class SplineFollow : MonoBehaviour
{
	[SerializeField] private SplineContainer path;
	[SerializeField] private float speed;

	private float t = 0f;

	void Update()
	{
		if (path == null) return;

		var spline = path.Spline;

		// advance along spline
		t += speed * Time.deltaTime;

		if (t > 1f)
			t -= 1f;

		// position on curve
		Vector3 position = path.EvaluatePosition(t);

		// direction of curve
		Vector3 tangent = path.EvaluateTangent(t);

		transform.position = position;
		transform.rotation = Quaternion.LookRotation(tangent);
	}

}
