using TMPro;
using UnityEngine;

public class SessionScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    void OnEnable()
    {
        GameEvents.OnSessionScoreChanged += UpdateScoreUI;
    }

    void OnDisable()
    {
        GameEvents.OnSessionScoreChanged -= UpdateScoreUI;
    }

    private void UpdateScoreUI(int score)
    {
        scoreText.text = score.ToString();
    }
}