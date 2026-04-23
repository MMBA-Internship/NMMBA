using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeaderboardScript : MonoBehaviour
{

    public List<GameObject> Names;
    public List<GameObject> Highscores;
    public SaveLoad SaveLoadManager;

    public void MakeLeaderboard()
    {
        /*int count = SaveLoadManager.retrievedData.Count;
        for(int i = 0; i < count; i++)
        {
            Highscores[i].GetComponent<TMP_Text>().text = new string($"{SaveLoadManager.retrievedData[i]}");
        }*/
    }

}
