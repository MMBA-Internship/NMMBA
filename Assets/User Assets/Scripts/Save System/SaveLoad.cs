using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

//Sets up a serializable list
[System.Serializable]
public class SerializableList<T>
{
    public List<T> list;
}

public class SaveLoad : MonoBehaviour
{
    //Iterates twice to create accessible lists via Json
    [SerializeField] private SerializableList<int> scores;
    [SerializeField] private SerializableList<int> highScores;
    //This score needs to be updated based on the players' scores at the end of the game
    public int score;

    private void Awake()
    {
        //Gets the file and data information. This is to make sure that every shutdown of the system doesn't get rid of all scores
        string json = File.ReadAllText(Application.dataPath + "/SavedData.json");
        if (!string.IsNullOrEmpty(json) && json != "{}")
        {
            scores = JsonUtility.FromJson<SerializableList<int>>(json);
        }

    }

    public void LoadData()
    {
        //Reads the Json file
        string json = File.ReadAllText(Application.dataPath + "/SavedData.json");
        
        //Checks if it's empty or not (I don't know if return does anything here, but I put it in just in case)
        if(string.IsNullOrEmpty(json) || json == "{}")
        {
            return;
        }
        
        //Sets the list "highscores" to the list found
        highScores = JsonUtility.FromJson<SerializableList<int>>(json);
        //Sorts the highscores lowest to highest. Could be done the other way maybe, but I don't get it fully
        highScores.list.Sort();

        /*for(int i = 0; i < highScores.list.Count; i++)//This is unnecessary. It's just for checking it inside editor
        {
            Debug.Log(highScores.list[i]);
        }*/

    }


    public void SaveData()
    {
        //Adds the score to the list
        scores.list.Add(score);
        //Sorts the scores lowest to highest
        scores.list.Sort();

        //Turns the scores into a string for Json
        string json = JsonUtility.ToJson(scores);
        //Saves it to a local space on the PC
        File.WriteAllText(Application.dataPath + "/SavedData.json", json);
    }
}
