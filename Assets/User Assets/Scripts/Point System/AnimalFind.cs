using System.Collections.Generic;
using UnityEngine;

public class AnimalFind : MonoBehaviour
{
    [Range(0, 360)] public float fovAngle = 60f;
    [SerializeField] private Camera viewPoint;
    [SerializeField] private string fishTag = "Enemy";
    [SerializeField] private LayerMask obstructionLayers;
    [SerializeField] private GameObject playerObj;
    [SerializeField] private float cameraDistance = 30f;

    private void Awake()
    {
        if (viewPoint == null)
            viewPoint = Camera.main;

        if (playerObj == null && viewPoint != null)
            playerObj = viewPoint.gameObject;
    }

    public List<AnimalVisibilityInfo> GetFishVisibilityData()
    {
        List<AnimalVisibilityInfo> visibleFishData = new List<AnimalVisibilityInfo>();

        if (playerObj == null || viewPoint == null)
        {
            Debug.LogError("AnimalFind: playerObj or viewPoint is missing.");
            return visibleFishData;
        }

        Collider[] availableTargets = Physics.OverlapSphere(playerObj.transform.position, cameraDistance);

        foreach (Collider target in availableTargets)
        {
            if (!target.gameObject.CompareTag(fishTag))
                continue;

            Vector3 directionToTarget = (target.transform.position - viewPoint.transform.position).normalized;
            float angle = Vector3.Angle(viewPoint.transform.forward, directionToTarget);

            bool isInFrame = angle <= fovAngle * 0.5f;
            if (!isInFrame)
                continue;

            bool isObstructed = Physics.Linecast(
                viewPoint.transform.position,
                target.transform.position,
                obstructionLayers,
                QueryTriggerInteraction.Ignore
            );

            FishData fishData = target.GetComponent<FishData>();

            AnimalVisibilityInfo info = new AnimalVisibilityInfo
            {
                fishObject = target.gameObject,
                fishData = fishData,
                isInFrame = isInFrame,
                isObstructed = isObstructed
            };

            visibleFishData.Add(info);
        }

        return visibleFishData;
    }
}