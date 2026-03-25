using System.Collections.Generic;
using UnityEngine;

public class SessionScoreManager : MonoBehaviour
{
    [SerializeField] private List<SinglePhotoScoreResult> photoResults = new List<SinglePhotoScoreResult>();

    public int TotalSessionScore { get; private set; } // CHANGED
    public string HighestScoringPictureName { get; private set; } = "None"; // ADDED
    public int HighestPictureScore { get; private set; } = 0; // ADDED

    private void OnEnable()
    {
        // CHANGED
        GameEvents.OnPhotoScored += AddPhotoResult;
    }

    private void OnDisable()
    {
        GameEvents.OnPhotoScored -= AddPhotoResult;
    }

    // CHANGED
    private void AddPhotoResult(SinglePhotoScoreResult result)
    {
        photoResults.Add(result);
        TotalSessionScore += result.totalScore;

        if (result.totalScore > HighestPictureScore)
        {
            HighestPictureScore = result.totalScore;
            HighestScoringPictureName = result.pictureName;
        }

        GameEvents.RaiseSessionScoreChanged(TotalSessionScore);
    }

    // ADDED: useful for ending screen access
    public List<SinglePhotoScoreResult> GetAllPhotoResults()
    {
        return photoResults;
    }
}