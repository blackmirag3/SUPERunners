using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public KillCounter kill;

    public int EnemyPerLevel { get; private set; }
    [SerializeField]
    private int startEnemyCount;
    [SerializeField]
    private int maxEnemies;

    public bool GameStart { get; private set; }
    private bool levelWon;
    public GameObject winScreen;

    public GameEvent onLevelComplete;

    private void Start()
    {
        EnemyPerLevel = startEnemyCount;

        GameStart = false;
        levelWon = false;
        
        RebuildArena();
    }

    private void Update()
    {
        levelWon = kill.LevelWon;
        if (levelWon)
        {
            StopGame();
        }
    }

    public void StartGame()
    {
        GameStart = true;
    }

    public void StopGame()
    {
        GameStart = false;
        winScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void RebuildArena()
    {
        winScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        onLevelComplete.CallEvent(this, EnemyPerLevel < maxEnemies ? EnemyPerLevel++ : maxEnemies);
    }
}
