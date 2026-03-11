using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AnimalFind : MonoBehaviour
{
    [Range(0, 360)] public float fovAngle;
    [SerializeField] Camera viewPoint;
    Image targetReference;
    [SerializeField] Canvas TargetCanvas;
    LayerMask ignoreLayers;
    [SerializeField] Canvas HUD;
    GameObject PlayerObj;
    float cameraDistance;
    private void Awake()
    {
        // TODO Need to specify the layers that are important to check between
        //ignoreLayers = LayerMask.GetMask("Player") | LayerMask.GetMask("Ground") | LayerMask.GetMask("Obstacle") | LayerMask.GetMask("Wall");
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            List<GameObject> fish = new List<GameObject>();
            List<float> angleToFish = new List<float>();
            Collider[] AvailableTargets = Physics.OverlapSphere(PlayerObj.transform.position, cameraDistance);
            foreach (Collider target in AvailableTargets)
            {
                if (target.gameObject.CompareTag("Enemy"))
                {
                    Vector3 directionToTarget = (target.transform.position - PlayerObj.transform.position).normalized;
                    float signedAngle = Vector3.Angle(viewPoint.transform.forward, directionToTarget);
                    if (!Physics.Linecast(PlayerObj.transform.position, target.gameObject.transform.position, ignoreLayers, QueryTriggerInteraction.Ignore))
                    {
                        fish.Add(target.gameObject);
                        angleToFish.Add(signedAngle);
                    }
                    for (int i = 0; i < fish.Count; i++)
                    {
                        if (Mathf.Abs(angleToFish[i]) > fovAngle * 1.5)
                        {
                            fish.RemoveAt(i);
                            angleToFish.RemoveAt(i);
                        }
                    }
                }
            }
           
        }
    }

}