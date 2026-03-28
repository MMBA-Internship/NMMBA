using UnityEngine;

public class CameraToObject : MonoBehaviour
{
    public Transform target;


    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position;
    }
}
