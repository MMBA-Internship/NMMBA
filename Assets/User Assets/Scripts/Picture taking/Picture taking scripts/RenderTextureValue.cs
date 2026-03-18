using UnityEngine;

[CreateAssetMenu(fileName = "RenderTextureValue", menuName = "Scriptable Objects/RenderTextureValue")]
public class RenderTextureValue : ScriptableObject
{
    [System.NonSerialized]
    public RenderTexture value;
}
