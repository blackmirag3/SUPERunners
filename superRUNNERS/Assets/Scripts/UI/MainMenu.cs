using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject currentMenu;
    public GameObject settingsMenu;
    public bool gameIsPaused;

    private KeyCode escapeKey = KeyCode.Escape;

    private void Start()
    {
        gameIsPaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(escapeKey))
        {
            CloseSettings();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level0");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        currentMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void CloseSettings()
    {
        currentMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }
}
