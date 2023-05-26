using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public KillCounter kill;

    public bool GameStart { get; private set; }
    private bool levelWon;
    public GameObject winScreen;

    // Start is called before the first frame update
    private void Start()
    {
        GameStart = false;
        levelWon = false;
        winScreen.SetActive(false);
    }

    // Update is called once per frame
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

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
