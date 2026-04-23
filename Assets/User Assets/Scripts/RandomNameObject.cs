using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LocalisedWord
{
    public string english;
    public string chineseSimplified;
}
[CreateAssetMenu(fileName = "RandomNameObject", menuName = "Scriptable Objects/RandomNameObject")]
public class RandomNameObject : ScriptableObject
{
    public List<LocalisedWord> adjectives;
    public List<LocalisedWord> fish;
}
