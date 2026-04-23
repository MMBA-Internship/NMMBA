using UnityEngine;

public enum Language
{
    English,
    Chinese,
}

public class NameGenerator : MonoBehaviour
{
    public RandomNameObject dataBase;
    public Language currentLanguage;

    public string GeneratedName()
    {
        if(dataBase == null || dataBase.adjectives.Count == 0 || dataBase.fish.Count == 0)
        {
            return "InvalidName";
        }
        var adj = dataBase.adjectives[Random.Range(0, dataBase.fish.Count - 1)];
        var fish = dataBase.fish[Random.Range(0, dataBase.adjectives.Count - 1)];

        return FormatName(adj, fish);
    }

    private string FormatName(LocalisedWord adj, LocalisedWord fish)
    {
        switch (currentLanguage)
        {
            case Language.English:
                return adj.english + " " + fish.english;
            default:
                return adj.chineseSimplified + fish.chineseSimplified;
        }
    }

    public void SwitchLanguageEnglish()
    {
        currentLanguage = Language.English;
    }

    public void SwitchLanguageTaiwan()
    {
        currentLanguage = Language.Chinese;
    }
}
