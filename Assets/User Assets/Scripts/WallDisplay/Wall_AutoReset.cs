using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AutoReset : MonoBehaviour
{
    [SerializeField] private float resetTime = 150f;

    void Start()
    {
        StartCoroutine(ResetLoop());
    }

    IEnumerator ResetLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(resetTime);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}