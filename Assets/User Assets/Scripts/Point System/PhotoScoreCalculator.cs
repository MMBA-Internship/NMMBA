using System.Collections.Generic;
using UnityEngine;

public class PhotoScoreCalculator : MonoBehaviour
{
    [SerializeField] private AnimalFind animalFind;
    [SerializeField] private SessionScoreManager sessionScoreManager;

    public void ScorePhoto()
    {
        List<GameObject> visibleFish = animalFind.GetVisibleFish();

        SinglePhotoScoreResult result = new SinglePhotoScoreResult();

        foreach (GameObject fishGO in visibleFish)
        {
            //  SCORE CALCULATION LOGIC HERE

            FishData fishData = fishGO.GetComponent<FishData>();
            if(fishData == null)   continue;

            /*result.pictureTexture = animalFind.GetPhotoTexture();
            result.picturePath = animalFind.GetPhotoPath();
            result.totalScore += fishInstance.FishData.scoreAmount;*/

        }
        sessionScoreManager.AddPhotoResult(result);

    }


}