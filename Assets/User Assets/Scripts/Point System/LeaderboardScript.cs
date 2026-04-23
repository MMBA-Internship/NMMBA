using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeaderboardScript : MonoBehaviour
{

    public List<GameObject> Names;
    public List<GameObject> Highscores;
    public SaveLoad SaveLoadManager;
    List<int> highscores;
    List<string> names;
    private void Awake()
    {
        names = new List<string>();
        highscores = new List<int>();
    }
    public void MakeLeaderboard()
    {
        int count = Highscores.Count;
        for(int i = 0; i < count; i++)
        {
            highscores.Add(SaveLoadManager.savedData.list[i].score);
            names.Add(SaveLoadManager.savedData.list[i].name);
            Highscores[i].GetComponent<TMP_Text>().text = new string($"{highscores[i]}");
            Names[i].GetComponent<TMP_Text>().text = new string($"{names[i]}");
        }
    }

}
