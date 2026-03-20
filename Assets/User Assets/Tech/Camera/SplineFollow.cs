using UnityEngine;

public class SplineFollow : MonoBehaviour
{
    [SerializeField] private BezierCurve bezierCurve;
    [SerializeField] private float speed;
    [SerializeField] private float pointRange;

    private Vector3[] points;
    private int currentTargetIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        points = bezierCurve.GetPoints();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(CalculateCurveDirection().normalized);

        transform.Translate(speed * CalculateCurveDirection().normalized, Space.World);
    }

    private Vector3 CalculateCurveDirection()
    {
        if (bezierCurve == null || points.Length == 0) return Vector3.zero;

        if (Vector3.Distance(transform.position, points[currentTargetIndex]) < pointRange)
        {
            if (points.Length > currentTargetIndex)
            {
                currentTargetIndex = (currentTargetIndex + 1);
            }
            if (currentTargetIndex >= points.Length)
            {
                currentTargetIndex = 0;
            }
        }

        Vector3 targetDirection = points[currentTargetIndex] - transform.position;
        Vector3 moveDirection = Vector3.Normalize(targetDirection);

        return moveDirection;
    }

}
