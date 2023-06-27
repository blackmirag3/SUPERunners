using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject currentMenu;
    [SerializeField]
    private GameObject settingsMenu;
    [SerializeField]
    private GameObject diffSelector;

    private KeyCode escapeKey = KeyCode.Escape;

    private void Update()
    {
        if (Input.GetKeyDown(escapeKey))
        {
            OpenMainMenu();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Arena");
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

    public void OpenMainMenu()
    {
        currentMenu.SetActive(true);
        settingsMenu.SetActive(false);
        diffSelector.SetActive(false);
    }

    public void SelectDifficulty()
    {
        currentMenu.SetActive(false);
        diffSelector.SetActive(true);
    }
}
