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
    [SerializeField]
    private GameObject keybindMenu;

    private bool rebinding;

    private KeyCode escapeKey = KeyCode.Escape;

    private void Start()
    {
        rebinding = false;
        menuState = CurrentMenu.main;
    }

    private enum CurrentMenu
    {
        main,
        play,
        settings,
        keybinds,
    }
    private CurrentMenu menuState;

    private void Update()
    {
        if (Input.GetKeyDown(escapeKey))
        {
            OnEscapePress();
        }
    }

    private void OnEscapePress()
    {
        switch (menuState)
        {
            case CurrentMenu.main:
                break;
            case CurrentMenu.play:
                menuState = CurrentMenu.main;
                OpenMenu(menuState);
                break;
            case CurrentMenu.settings:
                menuState = CurrentMenu.main;
                OpenMenu(menuState);
                break;
            case CurrentMenu.keybinds:
                if (!rebinding)
                {
                    menuState = CurrentMenu.settings;
                    OpenMenu(menuState);
                }
                break;
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
        menuState = CurrentMenu.settings;
        OpenMenu(menuState);
    }

    public void OpenMainMenu()
    {
        menuState = CurrentMenu.main;
        OpenMenu(menuState);
    }

    public void SelectDifficulty()
    {
        menuState = CurrentMenu.play;
        OpenMenu(menuState);
    }

    public void OpenKeyBinds()
    {
        menuState = CurrentMenu.keybinds;
        OpenMenu(menuState);
    }

    public void RebindingActive()
    {
        Debug.Log("esc key disabled");
        rebinding = true;
    }

    public void RebindingDone()
    {
        Debug.Log("esc key enabled");
        StartCoroutine(ResetEsc());
    }

    private IEnumerator ResetEsc()
    {
        yield return null;
        rebinding = false;
    }

    private void OpenMenu(CurrentMenu menu)
    {
        switch (menu)
        {
            case CurrentMenu.main:
                currentMenu.SetActive(true);
                diffSelector.SetActive(false);
                settingsMenu.SetActive(false);
                break;
            case CurrentMenu.play:
                currentMenu.SetActive(false);
                diffSelector.SetActive(true);
                break;
            case CurrentMenu.settings:
                currentMenu.SetActive(false);
                keybindMenu.SetActive(false);
                settingsMenu.SetActive(true);
                break;
            case CurrentMenu.keybinds:
                settingsMenu.SetActive(false);
                keybindMenu.SetActive(true);
                break;
        }
    }
}
