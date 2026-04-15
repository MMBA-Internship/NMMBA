using System.Collections.Generic;
using Intel.RealSense;
using TMPro;
using Unity.Android.Gradle.Manifest;
using Unity.Hierarchy;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.UIElements;

public class PhotoScoreCalculator : MonoBehaviour
{
    [SerializeField] private AnimalFind animalFind;
    [SerializeField] private float idealDistanceFromCam = 3f;
    [SerializeField] private float maxDistanceFromCam = 10f;
    [SerializeField] private float baseScoreDistance = 40f;
    [SerializeField] private float baseScoreCentering = 30f;
    [SerializeField] private float baseScoreObstruction = 30f;

    [SerializeField] private TextMeshProUGUI scoreUI;

    private static int pictureCounter = 0;
    private string foundFishReferences = "";

    //Measure distance from centre of frame -  Linear falloff of points based on distance from centre
    //  Fish exactly centred = 300 points
    //  Fish at edge of frame = 0 points

    // multiply by raity from fish data

    /*
    Rarity Multiplier
    Base score is multiplied by species rarity: 
    Common(6 species) : 1.0x
    Uncommon(4 species): 1.2x
    Rare(3 species): 1.5x
    Extraordinary(2 species): 2.0x
    */

    // 1st discovery bonus:

    /*
     When photographing a species for the first time, add a discovery bonus scaled to rarity: 
    Common: +100 points
    Uncommon: +150 points
    Rare: +300 points
    Extraordinary: +500 points
    */


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

        Debug.Log(pictureName);

        int totalPictureScore = 0;

        foreach (AnimalFindInfo fishInfo in fishVisibilityData)
        {
            if (fishInfo.fishData == null)
            {
                Debug.Log($"FishData is null for fish object: {fishInfo.fishObject.name}");
                continue;
            }

            if (!fishInfo.isInFrame || fishInfo.distance >= maxDistanceFromCam)
            {
                Debug.Log($"Fish {fishInfo.fishData.reference} is not in frame or too far (Distance: {fishInfo.distance}). Skipping scoring for this fish.");
                continue;
            }

            float fishScore = 0;
            float obstructionScore;
            float distanceScore;
            float centeringScore;

            if (!fishInfo.isObstructed)
                //fishScore += baseScoreObstruction;
                obstructionScore = baseScoreObstruction;
            else
                obstructionScore = baseScoreObstruction * 0.5f;

            // distance calculation
            float distanceFromIdeal = Mathf.Abs(fishInfo.distance - idealDistanceFromCam);
            float maxDistanceDeviation = Mathf.Max(idealDistanceFromCam, maxDistanceFromCam - idealDistanceFromCam);

            float falloffAmt = Mathf.Clamp01(1f - (distanceFromIdeal / maxDistanceDeviation));
            distanceScore = baseScoreDistance * falloffAmt;

            // centering calculation
            float centeringFalloffAmt = Mathf.Clamp01(1 - (fishInfo.angle / animalFind.fovAngle));
            centeringScore = baseScoreCentering * centeringFalloffAmt;

            fishScore = obstructionScore + distanceScore + centeringScore;

            float rarityMultiplier = ((int)fishInfo.fishData.rarity) / 10f;

            float discoveryBonus = 0;

            if (!foundFishReferences.Contains(fishInfo.fishData.reference))
            {
                foundFishReferences += fishInfo.fishData.reference + ",";
                switch (fishInfo.fishData.rarity)
                {
                    case FishData.Rarity.Common:
                        discoveryBonus = 100f;
                        break;
                    case FishData.Rarity.Uncommon:
                        discoveryBonus = 150f;
                        break;
                    case FishData.Rarity.Rare:
                        discoveryBonus = 300f;
                        break;
                    case FishData.Rarity.Extraordinary:
                        discoveryBonus = 500f;
                        break;
                }
            }

            totalPictureScore += Mathf.RoundToInt(fishScore * rarityMultiplier + discoveryBonus) * 10;

            Debug.Log(
                $"{pictureName} -> Fish: {fishInfo.fishData.reference} | " +
                $"Obstructed: {fishInfo.isObstructed} => {obstructionScore}pts/{baseScoreObstruction} | " +
                $"Distance: {fishInfo.distance}/{falloffAmt} => {distanceScore}pts/{baseScoreDistance} | " +
                $"Centered: {fishInfo.angle}/{centeringFalloffAmt} => {centeringScore}pts/{baseScoreCentering} | " +
                $"Rarity: {fishInfo.fishData.rarity} => x{rarityMultiplier} | " +
                $"Discovery Bonus: {discoveryBonus}pts | {foundFishReferences}" +
                $"Total Score: {fishScore}"
            );
        }

        SinglePhotoScoreResult result = new SinglePhotoScoreResult();
        result.pictureName = pictureName;
        result.pictureTexture = photo;
        result.picturePath = "";
        result.totalScore = totalPictureScore;

        Debug.Log($"{pictureName} total score = {totalPictureScore}");
        scoreUI.text = totalPictureScore.ToString();

        GameEvents.RaisePhotoScored(result);
    }
}