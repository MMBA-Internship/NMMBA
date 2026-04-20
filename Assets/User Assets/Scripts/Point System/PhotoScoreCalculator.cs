using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;

public class PhotoScoreCalculator : MonoBehaviour
{
    [SerializeField] private AnimalFind animalFind;
    [SerializeField] private float idealDistanceFromCam = 3f;
    [SerializeField] private float maxDistanceFromCam = 10f;
    [SerializeField] private float baseScoreDistance = 40f;
    [SerializeField] private float baseScoreCentering = 30f;
    [SerializeField] private float baseScoreObstruction = 30f;

    private static int pictureCounter = 0;
    private List<string> foundFishReferences = new List<string>();

    //Measure distance from centre of frame -  Linear falloff of points based on distance from centre
    //  Fish exactly centred = 300 points
    //  Fish at edge of frame = 0 points

    // multiply by raity from fish data

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

        List<AnimalFindInfo> fishVisibilityData = animalFind.GetFishVisibilityData();

        pictureCounter++;
        string pictureName = "Picture" + pictureCounter;

        Debug.Log($"Picture {pictureCounter}: {pictureName}");

        int totalPictureScore = 0;

        foreach (AnimalFindInfo info in fishVisibilityData)
        {
            if (info.fishData == null)
            {
                Debug.Log($"FishData is null for fish object: {info.fishObject.name}");
                continue;
            }

            if (!info.isInFrame || info.distance >= maxDistanceFromCam)
            {
                Debug.Log($"Fish {info.fishData.reference} is not in frame or too far (Distance: {info.distance}). Skipping scoring for this fish.");
                continue;
            }

            float fishScore = 0;
            float obstructionScore;
            float distanceScore;
            float centeringScore;

            if (!info.isObstructed)
                //fishScore += baseScoreObstruction;
                obstructionScore = baseScoreObstruction;
            else
                obstructionScore = baseScoreObstruction * 0.5f;

            // distance linear falloff calc
            float distanceFromIdeal = Mathf.Abs(info.distance - idealDistanceFromCam);
            float maxDistanceDeviation = Mathf.Max(idealDistanceFromCam, maxDistanceFromCam - idealDistanceFromCam);

            float falloffAmt = Mathf.Clamp01(1f - (distanceFromIdeal / maxDistanceDeviation));
            distanceScore = baseScoreDistance * falloffAmt;

            // centering linear falloff calc
            float centeringFalloffAmt = Mathf.Clamp01(1 - (info.angle / animalFind.fovAngle));
            centeringScore = baseScoreCentering * centeringFalloffAmt;

            fishScore = obstructionScore + distanceScore + centeringScore;

            totalPictureScore += Mathf.RoundToInt(fishScore * ((int)info.fishData.rarity/10f)) * 10;

            Debug.Log(
                $"{pictureName} | Fish: {info.fishData.reference}\n" +
                $"Obstructed: {info.isObstructed} => {obstructionScore}pts | Distance: {info.distance}/{falloffAmt} => {distanceScore}pts | Centered: {info.angle}/{centeringFalloffAmt} => {centeringScore}pts\n" +
                $"Total Score: {fishScore}"
            );
        }

        SinglePhotoScoreResult result = new SinglePhotoScoreResult();
        result.pictureName = pictureName;
        result.pictureTexture = photo;
        result.picturePath = "";
        result.totalScore = totalPictureScore;

        Debug.Log($"{pictureName} total score = {totalPictureScore}");

        GameEvents.RaisePhotoScored(result);
    }
}