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
    private TextMeshProUGUI finalKillScore;
    [SerializeField]
    private TextMeshProUGUI finalRoomScore;
    [SerializeField]
    private TextMeshProUGUI kHighScore;
    [SerializeField]
    private TextMeshProUGUI rHighScore;
    [SerializeField]
    private TextMeshProUGUI newAlert;

    [SerializeField]
    private GameEvent buildNewArena;
    [SerializeField]
    private GameEvent onPause;
    [SerializeField]
    private GameEvent updateEnemySpawnPool;

    private byte poolPhase;

    [SerializeField]
    private DifficultySettings diff;
    private DifficultySettings initialDiffValues;

    // score stuff
    private int killHighScore, roomHighScore;
    private readonly string easyFilename = "score00.dat";
    private readonly string normalFilename = "score01.dat";
    private readonly string hardFilename = "score02.dat";

    private void Awake()
    {
        if (DifficultySelector.instance != null)
        {
            diff = DifficultySelector.instance.selectedDifficulty;
        }
        initialDiffValues = Instantiate(diff);

        startEnemyCount = diff.startEnemySpawn;
        maxEnemies = diff.maxEnemySpawn;

        newAlert.enabled = false;
        ObtainHighscores(diff.difficulty);
    }

    private void Start()
    {
        EnemyPerLevel = startEnemyCount;

        StartGame();
        levelWon = false;
        areasCleared = 0;
        poolPhase = 0;
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
        TextMeshProUGUI clearText = winScreen.GetComponentInChildren<TextMeshProUGUI>();
        if (clearText != null)
        {
            clearText.SetText($"Room {areasCleared} Cleared");
        }
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
        DisplayAndCheckScores();

        gameOverScreen.SetActive(true);
    }

    private void DisplayAndCheckScores()
    {
        int killedEnemies = kill.enemiesKilled;
        finalKillScore.SetText(killedEnemies.ToString());
        finalRoomScore.SetText(areasCleared.ToString());

        bool highscoreUpdated = false;
        if (killedEnemies > killHighScore)
        {
            killHighScore = killedEnemies;
            highscoreUpdated = true;
        }
        if (areasCleared > roomHighScore)
        {
            roomHighScore = areasCleared;
            highscoreUpdated = true;
        }
        kHighScore.SetText(killHighScore.ToString());
        rHighScore.SetText(roomHighScore.ToString());

        if (highscoreUpdated)
        {
            newAlert.enabled = true;
            SaveNewHighscores(diff.difficulty);
        }
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

        if (poolPhase < 4)
        {
            UpdateSpawnPool(diff.difficulty, areasCleared);
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

    private void UpdateSpawnPool(Difficulty currDiff, int clearedAreas)
    {
        switch (currDiff)
        {
            case Difficulty.easy:
                if (clearedAreas == 3 || clearedAreas == 5 || clearedAreas == 7 || clearedAreas == 9)
                {
                    updateEnemySpawnPool.CallEvent(this, null);
                    poolPhase++;
                }
                break;
            case Difficulty.normal:
                if (clearedAreas == 1 || clearedAreas == 3 || clearedAreas == 4 || clearedAreas == 6)
                {
                    updateEnemySpawnPool.CallEvent(this, null);
                    poolPhase++;
                }
                break;
            case Difficulty.hard:
                if (clearedAreas == 1 || clearedAreas == 2 || clearedAreas == 3 || clearedAreas == 4)
                {
                    updateEnemySpawnPool.CallEvent(this, null);
                    poolPhase++;
                }
                break;
        }
        Debug.Log($"Enemy pool updated {poolPhase} times");
    }

    private void ObtainHighscores(Difficulty difficulty)
    {
        string fileName = null;
        switch (difficulty)
        {
            case Difficulty.easy:
                fileName = easyFilename;
                break;
            case Difficulty.normal:
                fileName = normalFilename;
                break;
            case Difficulty.hard:
                fileName = hardFilename;
                break;
        }
        
        if (FileManager.LoadFromFile(fileName, out var json))
        {
            Score score = new Score();
            score.LoadJson(json);

            killHighScore = score.KillScore;
            roomHighScore = score.RoomScore;

            Debug.Log("Load successful");
        }
        else
        {
            killHighScore = 0;
            roomHighScore = 0;
        }
    }

    private void SaveNewHighscores(Difficulty difficulty)
    {
        string fileName = null;
        switch (difficulty)
        {
            case Difficulty.easy:
                fileName = easyFilename;
                break;
            case Difficulty.normal:
                fileName = normalFilename;
                break;
            case Difficulty.hard:
                fileName = hardFilename;
                break;
        }

        Score newScore = new Score();
        newScore.KillScore = killHighScore;
        newScore.RoomScore = roomHighScore;

        if (FileManager.WriteToFile(fileName, newScore.ToJson()))
        {
            Debug.Log("Save successful");
        }
    }
}
