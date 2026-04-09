using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GalleryScrolling : MonoBehaviour
{
    public RawImage photoDisplay;
    public TextMeshProUGUI scoreLabel;

    public SessionScoreManager scoreManager;

    private List<SinglePhotoScoreResult> allPhotos = new List<SinglePhotoScoreResult>();
    private SinglePhotoScoreResult currentPhoto;
    private int currentPhotoIndex = 0;

    public void OnEnable()
    {
        GameEvents.OnGalleryScreenActivated += GameEnded;
        Debug.Log("subscribed to event");
    }

    public void OnDisable()
    {
        GameEvents.OnGalleryScreenActivated -= GameEnded;
    }

    private void GameEnded()
    {
        allPhotos = scoreManager.GetAllPhotoResults();

        allPhotos.Sort((a, b) => b.totalScore.CompareTo(a.totalScore));

        Debug.Log("All photos and their scores:");
        foreach (SinglePhotoScoreResult photo in allPhotos)
        {
            Debug.Log($"Photo: {photo.pictureName}, Score: {photo.totalScore}");
        }

        // WATCH OUT IF PPL DONT TAKE ANY PHOTOS, THIS WILL CRASH !!!!!!

        currentPhotoIndex = 0;
        SetPhotoAndScore(currentPhotoIndex);

    }

    //TEST THIS

    public void NextPhoto()
    {
        currentPhotoIndex++;
        if (currentPhotoIndex >= allPhotos.Count)
        {
            currentPhotoIndex = 0;
        }

        SetPhotoAndScore(currentPhotoIndex);

    }

    public void PreviousPhoto()
    {
        currentPhotoIndex--;
        if (currentPhotoIndex < 0)
        {
            currentPhotoIndex = allPhotos.Count - 1;
        }
        SetPhotoAndScore(currentPhotoIndex);
    }


    private void SetPhotoAndScore(int index)
    {
        photoDisplay.texture = allPhotos[index].pictureTexture;
        scoreLabel.text = allPhotos[index].totalScore.ToString();

        Debug.Log($"Currently displaying picture {currentPhotoIndex} -> Name: {allPhotos[currentPhotoIndex].pictureName}, Score: {allPhotos[currentPhotoIndex].totalScore}");
    }




}
