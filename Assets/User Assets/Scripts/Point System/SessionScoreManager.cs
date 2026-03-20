using System.Collections.Generic;
using UnityEngine;

public class SessionScoreManager : MonoBehaviour
{
    public int TotalSessionScore { get; private set; }

    private List<SinglePhotoScoreResult> photoResults = new();

    public List<SinglePhotoScoreResult> GetResults()
    {
        return photoResults;
    }


    public void AddPhotoResult(SinglePhotoScoreResult result)
    {
        photoResults.Add(result);
        TotalSessionScore += result.totalScore;
        Debug.Log($"Photo score: {result.totalScore}, session total: {TotalSessionScore}");
    }
   

}