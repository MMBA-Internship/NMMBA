using UnityEngine;

[CreateAssetMenu(fileName = "Texture2DValue", menuName = "Scriptable Objects/Texture2DValue")]
public class Texture2DValue : ScriptableObject
{
    [System.NonSerialized]
    public Texture2D value;
}
