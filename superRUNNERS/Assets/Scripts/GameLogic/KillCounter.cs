using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillCounter : MonoBehaviour
{
    public float needToKill;
    public int leftToKill = 0;

    public TextMeshProUGUI countDisplay;
    public GameObject victoryScreen;

    public bool LevelWon { get; private set; }

    private void Start()
    {
        leftToKill = (int)needToKill;
        LevelWon = false;
    }

    private void Update()
    {
        if (!LevelWon)
        {
            CheckForWin();
            UpdateTextCol();
            countDisplay.SetText(leftToKill.ToString());
        }
    }
    
    private void UpdateTextCol()
    {
        if (leftToKill > needToKill * 0.66)
        {
            // Green
            countDisplay.color = new Color32(0, 255, 0, 255);
        }
        else if (leftToKill > needToKill * 0.33)
        {
            // Yellow
            countDisplay.color = new Color32(255, 255, 0, 255);
        }
        else
        {
            // Red
            countDisplay.color = new Color32(255, 0, 0, 255);
        }
    }

    private void CheckForWin()
    {
        if (leftToKill == 0)
        {
            LevelWon = true;
            victoryScreen.SetActive(true);
        }
    }
}
