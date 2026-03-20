using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AnimalFind : MonoBehaviour
{
    [Range(0, 360)] public float fovAngle;
    [SerializeField] private Camera viewPoint;
    [SerializeField] private string fishlayerName = "Enemy";
    [SerializeField] private LayerMask ignoreLayers;
    [SerializeField] private GameObject playerObj;
    [SerializeField] private float cameraDistance = 30f;

    
    //Image targetReference;
    //[SerializeField] Canvas TargetCanvas;
    //[SerializeField] Canvas HUD;

    private void Awake()
    {
        // TODO Need to specify the layers that are important to check between
        //ignoreLayers = LayerMask.GetMask("Player") | LayerMask.GetMask("Ground") | LayerMask.GetMask("Obstacle") | LayerMask.GetMask("Wall");
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            List<GameObject> fish = GetVisibleFish();
            Debug.Log("Visible Fish: " + fish.Count);


        }
    }

    public List <GameObject> GetVisibleFish()
    {
        List<GameObject> fish = new List<GameObject>();
        List<float> angleToFish = new List<float>();

        Collider[] availableTargets = Physics.OverlapSphere(playerObj.transform.position, cameraDistance);

        foreach (Collider target in availableTargets)
        {
            if(!target.gameObject.CompareTag(fishlayerName)) continue;
            
            Vector3 directionToTarget = (target.transform.position - playerObj.transform.position).normalized;
            float angle = Vector3.Angle(viewPoint.transform.forward, directionToTarget);


            //RESEARCHHHHHH
            //how toget percentage of how much of the fish is visible in the camera view
            if (!Physics.Linecast(playerObj.transform.position, target.gameObject.transform.position, ignoreLayers, QueryTriggerInteraction.Ignore))
            {
                fish.Add(target.gameObject);
                angleToFish.Add(angle);
            }
        }

        for (int i = fish.Count - 1; i >= 0; i--)
        {
            if (angleToFish[i] > fovAngle * 0.5f)
            {
                fish.RemoveAt(i);
                angleToFish.RemoveAt(i);
            }

        }

        return fish;
    }


}