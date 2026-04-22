using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CooldownSpinner : MonoBehaviour
{
    [SerializeField] private float cooldownDuration = 1.5f;
    [SerializeField] private float rotationSpeed = 200f;

    private bool isSpinning = false;

    void OnEnable()
    {
        GameEvents.OnPhotoCaptureStarted += StartCooldown;
    }

    void OnDisable()
    {
        GameEvents.OnPhotoCaptureStarted -= StartCooldown;
    }

    void Update()
    {
        if (isSpinning)
        {
            transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
        }
    }

    void Start()
    {
        // Hide visually but stay active so OnEnable works
        GetComponent<Image>().enabled = false;
    }

    void StartCooldown()
    {
        StartCoroutine(CooldownRoutine());
    }

    IEnumerator CooldownRoutine()
    {
        GetComponent<Image>().enabled = true; // Show
        isSpinning = true;
        transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(cooldownDuration);

        isSpinning = false;
        GetComponent<Image>().enabled = false; // Hide
    }
}