using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Score
{
    public int KillScore;
    public int RoomScore;

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
}
