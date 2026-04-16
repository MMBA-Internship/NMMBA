using System.Collections.Generic;
using UnityEngine;

public class SessionScoreManager : MonoBehaviour
{
    public List<SinglePhotoScoreResult> photoResults = new List<SinglePhotoScoreResult>();

    public int TotalSessionScore { get; private set; } // CHANGED
    public string HighestScoringPictureName { get; private set; } = "None"; // ADDED
    public int HighestPictureScore { get; private set; } = 0; // ADDED

    private SinglePhotoScoreResult highestScorePicture;

    private void OnEnable()
    {
        // CHANGED
        GameEvents.OnPhotoScored += AddPhotoResult;
        GameEvents.OnScoreRequested += GetTotalSessionScore;
    }

    private void OnDisable()
    {
        GameEvents.OnPhotoScored -= AddPhotoResult;
        GameEvents.OnScoreRequested -= GetTotalSessionScore;
    }

    // CHANGED
    private void AddPhotoResult(SinglePhotoScoreResult result)
    {
        photoResults.Add(result);
        TotalSessionScore += result.totalScore;

        if (result.totalScore > HighestPictureScore)
        {
            highestScorePicture = result;
        }

        GameEvents.RaiseSessionScoreChanged(TotalSessionScore);
    }

    // ADDED: useful for ending screen access
    public List<SinglePhotoScoreResult> GetAllPhotoResults()
    {
        return photoResults;
    }

    public int GetTotalSessionScore()
    {
        int result = 0;
        photoResults.ForEach(a =>
        {
            result += a.totalScore;
        });
        return result;
    }

    public SinglePhotoScoreResult GetFinalPhotoDisplay()
    {
        return highestScorePicture;
    }
}