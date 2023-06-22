using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
    public GameObject hud;
    public GameObject gameOverScreen;
    public PauseMenu pauseMenu;
    [SerializeField]
    private TextMeshProUGUI finalScore;

    public GameEvent buildNewArena;
    public GameEvent onPause;

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

    public void PlayerKilled()
    {
        Time.timeScale = 0;
        pauseMenu.enabled = false;
        onPause.CallEvent(this, true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        hud.SetActive(false);
        finalScore.SetText(kill.enemiesKilled.ToString());
        gameOverScreen.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Leave()
    {
        // do nothing for now
    }
    
}
