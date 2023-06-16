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

    public bool GameInProgress { get; private set; }
    private bool levelWon;
    public GameObject winScreen;

    public GameEvent buildNewArena;

    private void Start()
    {
        EnemyPerLevel = startEnemyCount;

        StartGame();
        levelWon = false;
        
    }

    private void Update()
    {
        levelWon = kill.LevelWon;
        if (levelWon && GameInProgress)
        {
            StopGame();
        }
    }

    public void StartGame()
    {
        GameInProgress = true;
    }

    public void StopGame()
    {
        GameInProgress = false;
        winScreen.SetActive(true);
        
    }

    public void RebuildArena(Component sender, object data)
    {
        winScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        buildNewArena.CallEvent(this, EnemyPerLevel < maxEnemies ? EnemyPerLevel++ : maxEnemies);
        StartGame();
    }
}
