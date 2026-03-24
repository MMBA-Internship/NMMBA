using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject Tutorial_A;
    public GameObject NotReadyState;
    public GameObject ReadyState;


    public void OnTutorialAPress()
    {
       Tutorial_A.SetActive(true);

    }

    public void OnTutorialAExit()
    {
       Tutorial_A.SetActive(false);

    }

    public void OnReadyPress()
    {
        NotReadyState.SetActive(false);

        ReadyState.SetActive(true);

    }

    public void OnCancelPress()
    {
        NotReadyState.SetActive(true);

        ReadyState.SetActive(false);

    }

}
