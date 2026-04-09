using UnityEngine;

[System.Serializable]
public class SinglePhotoScoreResult
{
    public string pictureName;
    public int totalScore;
    public RenderTexture pictureTexture;
    public string picturePath;

    public SinglePhotoScoreResult() { }

    public SinglePhotoScoreResult(string pictureName, int totalScore, RenderTexture pictureTexture = null, string picturePath = "")
    {
        this.pictureName = pictureName;
        this.totalScore = totalScore;
        this.pictureTexture = pictureTexture;
        this.picturePath = picturePath;
    }
}