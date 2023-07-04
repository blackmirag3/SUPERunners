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
    public static int areasCleared;

    public bool GameInProgress { get; private set; }
    private bool levelWon;
    [SerializeField]
    private GameObject winScreen;
    [SerializeField]
    private GameObject hud;
    [SerializeField]
    private GameObject gameOverScreen;
    [SerializeField]
    private PauseMenu pauseMenu;
    [SerializeField]
    private TextMeshProUGUI finalScore;

    public GameEvent buildNewArena;
    public GameEvent onPause;

    [SerializeField]
    private DifficultySettings diff;
    private DifficultySettings initialDiffValues;

    private void Awake()
    {
        if (DifficultySelector.instance != null)
        {
            diff = DifficultySelector.instance.selectedDifficulty;
        }
        initialDiffValues = Instantiate(diff);

        startEnemyCount = diff.startEnemySpawn;
        maxEnemies = diff.maxEnemySpawn;
    }

    private void Start()
    {
        EnemyPerLevel = startEnemyCount;

        StartGame();
        levelWon = false;
        areasCleared = 0;   
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

        buildNewArena.CallEvent(this, EnemyPerLevel);
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
        ResetDiffValues();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ResetDiffValues()
    {
        diff.enemySpeed = initialDiffValues.enemySpeed;
        diff.enemyUnarmedSpeed = initialDiffValues.enemyUnarmedSpeed;
        diff.isAggro = initialDiffValues.isAggro;
    }

    public void AreaCleared()
    {

        areasCleared++;

        if (diff.enemySpeed < diff.maxEnemySpeed)
        {
            IncreaseEnemySpeed(diff.difficulty, areasCleared);
        }

        if (areasCleared % 2 != 0)
        {
            if (areasCleared < 4 && diff.difficulty == Difficulty.easy)
            {
                EnemyPerLevel++;
            }
            return;
        }

        // increase diff
        if (EnemyPerLevel < maxEnemies)
        {
            EnemyPerLevel = IncreaseEnemySpawn(EnemyPerLevel, diff.difficulty, areasCleared);
        }

    }
    
    private int IncreaseEnemySpawn(int currEnemySpawn, Difficulty currDiff, int clearedAreas)
    {
        int newEnemySpawn = currEnemySpawn;

        switch (currDiff)
        {
            case Difficulty.easy:
                newEnemySpawn++;
                break;
            case Difficulty.normal:
                if (clearedAreas / 2 > 3)
                {
                    newEnemySpawn += 3;
                }
                else
                {
                    newEnemySpawn += 2;
                }
                break;
            case Difficulty.hard:
                if (clearedAreas / 2 > 2)
                {
                    newEnemySpawn += 3;
                }
                else
                {
                    newEnemySpawn += 2;
                }
                break;
        }
        return newEnemySpawn;
    }

    private void IncreaseEnemySpeed(Difficulty currDiff, int clearedAreas)
    {
        float multiplier = diff.enemySpeedMultipliers;
        diff.enemySpeed *= multiplier;
        diff.enemyUnarmedSpeed *= multiplier;
        
        if (currDiff == Difficulty.easy && clearedAreas > 4)
        {
            diff.isAggro = true;
        }
    }
}
