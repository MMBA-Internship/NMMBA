using System.Collections.Generic;
using UnityEngine;

public class AnimalFind : MonoBehaviour
{
    [Range(0, 360)] public float fovAngle = 60f;
    [SerializeField] private Camera viewPoint;
    [SerializeField] private string fishTag = "Fish";
    [SerializeField] private LayerMask obstructionLayers;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private float cameraDistance = 30f;

    private void Awake()
    {
        if (viewPoint == null)
            viewPoint = Camera.main;

        if (playerCamera == null && viewPoint != null)
            playerCamera = viewPoint.gameObject;
    }

    public List<AnimalFindInfo> GetFishVisibilityData()
    {
        List<AnimalFindInfo> visibleFishData = new List<AnimalFindInfo>();

        if (playerCamera == null || viewPoint == null)
        {
            Debug.LogError("AnimalFind: playerObj or viewPoint is missing.");
            return visibleFishData;
        }

        Collider[] availableTargets = Physics.OverlapSphere(playerCamera.transform.position, cameraDistance);

        foreach (Collider target in availableTargets)
        {
            if (!target.gameObject.CompareTag(fishTag))
                continue;

            Vector3 directionToTarget = (target.transform.position - viewPoint.transform.position).normalized;
            float angle = Vector3.Angle(viewPoint.transform.forward, directionToTarget);
            // if angle is 0 => in front

            float distanceToTarget = Vector3.Distance(viewPoint.transform.position, target.transform.position);

            bool isInFrame = (angle <= (fovAngle * 0.5f));
            if (!isInFrame)
                continue;

            bool isObstructed = Physics.Linecast(
                viewPoint.transform.position,
                target.transform.position,
                obstructionLayers,
                QueryTriggerInteraction.Ignore
            );

            FishData fishData = target.GetComponent<FishData>();

            AnimalFindInfo info = new AnimalFindInfo
            {
                fishObject = target.gameObject,
                fishData = fishData,
                isInFrame = isInFrame,
                isObstructed = isObstructed,
                angle = angle,
                distance = distanceToTarget

            };

            visibleFishData.Add(info);
        }

        return visibleFishData;
    }
}