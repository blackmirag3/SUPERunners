using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices; 
[assembly: InternalsVisibleTo( "PlayMode" )]
[assembly: InternalsVisibleTo( "EditMode" )]
[assembly: InternalsVisibleTo( "TestInfrastructure" )]

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    internal GameObject currentMenu;
    [SerializeField]
    internal GameObject settingsMenu;
    [SerializeField]
    internal GameObject diffSelector;
    [SerializeField]
    internal GameObject keybindMenu;
    [SerializeField]
    internal GameObject gamemodeMenu;

    private bool rebinding;

    private KeyCode escapeKey = KeyCode.Escape;

    private void Start()
    {
        rebinding = false;
        menuState = CurrentMenu.main;
        /*
        if (currentMenu == null)
        currentMenu =  GameObject.Find("Main Menu");

        if (settingsMenu == null)
        settingsMenu = GameObject.Find("Settings Menu");

        if (diffSelector == null)
        diffSelector = GameObject.Find("Difficulty Selector");

        if (keybindMenu == null)
        keybindMenu = GameObject.Find("Keybind Menu");
        */
    }
    internal enum CurrentMenu
    {
        main,
        play,
        settings,
        keybinds,
        gameSelect,
    }

    internal CurrentMenu menuState;

    private void Update()
    {
        if (Input.GetKeyDown(escapeKey))
        {
            OnEscapePress();
        }
    }

    internal void OnEscapePress()
    {
        switch (menuState)
        {
            case CurrentMenu.main:
                break;
            case CurrentMenu.gameSelect:
                menuState = CurrentMenu.main;
                OpenMenu(menuState);
                break;
            case CurrentMenu.play:
                menuState = CurrentMenu.gameSelect;
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

    public void StartTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void QuitGame()
    {
        PlayerPrefs.Save();
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

    public void SelectGamemode()
    {
        menuState = CurrentMenu.gameSelect;
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
                gamemodeMenu.SetActive(false);
                settingsMenu.SetActive(false);
                break;
            case CurrentMenu.play:
                gamemodeMenu.SetActive(false);
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
            case CurrentMenu.gameSelect:
                currentMenu.SetActive(false);
                diffSelector.SetActive(false);
                gamemodeMenu.SetActive(true);
                break;
        }
    }
}
