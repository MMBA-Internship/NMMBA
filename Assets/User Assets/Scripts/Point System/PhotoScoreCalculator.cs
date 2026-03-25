using System.Collections.Generic;
using UnityEngine;

public class PhotoScoreCalculator : MonoBehaviour
{
    [SerializeField] private AnimalFind animalFind;

    private int pictureCounter = 0;

    private void Awake()
    {
        if (animalFind == null)
            animalFind = GetComponent<AnimalFind>();

        if (animalFind == null)
            animalFind = FindAnyObjectByType<AnimalFind>();    
    }

    private void OnEnable()
    {
        GameEvents.OnPictureTaken += ScorePhoto;
    }

    private void OnDisable()
    {
        GameEvents.OnPictureTaken -= ScorePhoto;
    }

    private void ScorePhoto(RenderTexture photo)
    {
        if (animalFind == null)
        {
            Debug.LogError("PhotoScoreCalculator: AnimalFind reference is missing.");
            return;
        }

        List<AnimalVisibilityInfo> fishVisibilityData = animalFind.GetFishVisibilityData();

        pictureCounter++;
        string pictureName = "Picture" + pictureCounter;

        int total = 0;

        foreach (AnimalVisibilityInfo info in fishVisibilityData)
        {
            if (info.fishData == null)
                continue;

            int fishScore = 0;

            if (info.isInFrame && !info.isObstructed)
            {
                fishScore = info.fishData.scoreAmount;
            }
            else if (info.isInFrame && info.isObstructed)
            {
                fishScore = Mathf.RoundToInt(info.fishData.scoreAmount * 0.5f);
            }

            total += fishScore;

            Debug.Log(
                $"{pictureName} | Fish: {info.fishObject.name} | InFrame: {info.isInFrame} | Obstructed: {info.isObstructed} | Score: {fishScore}"
            );
        }

        SinglePhotoScoreResult result = new SinglePhotoScoreResult();
        result.pictureName = pictureName;
        result.pictureTexture = null;
        result.picturePath = "";
        result.totalScore = total;

        Debug.Log($"{pictureName} total score = {total}");

        GameEvents.RaisePhotoScored(result);
    }
}