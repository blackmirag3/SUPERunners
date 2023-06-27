using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySelector : MonoBehaviour
{
    public DifficultySettings selectedDifficulty;

    public static DifficultySelector instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
    }

    public void SelectDifficulty(DifficultySettings diff)
    {
        selectedDifficulty = diff;
        Debug.Log($"Difficulty selected - " + selectedDifficulty.name);
    }

}
