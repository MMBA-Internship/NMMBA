using UnityEngine;

[System.Serializable]
public class SinglePhotoScoreResult
{
    // CHANGED / ADDED
    public string pictureName;
    public int totalScore;
    public Texture2D pictureTexture;
    public string picturePath;

    public SinglePhotoScoreResult() { }

    public SinglePhotoScoreResult(string pictureName, int totalScore, Texture2D pictureTexture = null, string picturePath = "")
    {
        this.pictureName = pictureName;
        this.totalScore = totalScore;
        this.pictureTexture = pictureTexture;
        this.picturePath = picturePath;
    }
}