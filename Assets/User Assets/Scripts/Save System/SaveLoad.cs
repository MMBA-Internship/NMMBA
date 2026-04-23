using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class ScoreEntry
{
    public int score;
    public string name;

    public ScoreEntry(int score, string name)
    {
        this.score = score;
        this.name = name;
    }
}

[System.Serializable]
public class SerializableList<T>
{
    public List<T> list = new List<T>();
}



public class SaveLoad : MonoBehaviour
{
    [SerializeField] public SerializableList<ScoreEntry> savedData = new SerializableList<ScoreEntry>();

    private string named;
    //This score needs to be updated based on the players' scores at the end of the game
    public int score;
    private string path;

    private void Awake()
    {
        path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/SavedData.json";
        LoadData();
    }

    public void LoadData()
    {
        if (!File.Exists(path))
        {
            savedData.list = new List<ScoreEntry>();
            return;
        }

        string json = File.ReadAllText(path);

        if (string.IsNullOrEmpty(json))
        {
            savedData.list = new List<ScoreEntry>();
            return;
        }


        savedData.list = JsonConvert.DeserializeObject<List<ScoreEntry>>(json)
            ?? new List<ScoreEntry>();
    }


    public void SaveData()
    {
        savedData.list.Add(new ScoreEntry(score, named));

        savedData.list.Sort((a,b) => b.score.CompareTo(a.score));

        string json = JsonConvert.SerializeObject(savedData.list, Formatting.Indented);
        File.WriteAllText(path, json);
    }

    public List<ScoreEntry> GetTopScores(int count)
    {
        return savedData.list.GetRange(0, Mathf.Min(count, savedData.list.Count));
    }

    public void SaveName(string Name)
    {
        named = Name;
    }
}
