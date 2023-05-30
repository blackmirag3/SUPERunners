using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject currentMenu;
    public GameObject settingsMenu;
    public bool gameIsPaused;

    private KeyCode escapeKey = KeyCode.Escape;
    private bool isOtherMenu;
    


    private void Start()
    {
        gameIsPaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(escapeKey))
        {
            if (isOtherMenu)
                CloseSettings();
            else if (!gameIsPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level0");
        ResumeGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        isOtherMenu = true;
        currentMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        isOtherMenu = false;
        currentMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void PauseGame()
    {
        currentMenu.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        currentMenu.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
