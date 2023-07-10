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
        else if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);
    }

    public void SelectDifficulty(DifficultySettings diff)
    {
        selectedDifficulty = Instantiate(diff);
        Debug.Log($"Difficulty selected - " + selectedDifficulty.name);
    }

}
