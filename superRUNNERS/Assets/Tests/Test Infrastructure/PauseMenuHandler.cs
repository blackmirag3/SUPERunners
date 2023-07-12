using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuHandler : MonoBehaviour
{
    [SerializeField] internal GameObject currentMenu;
    [SerializeField] internal GameObject settingsMenu;
    [SerializeField] internal GameObject keybindMenu;

    internal enum CurrentMenu
    {
        none,
        pause,
        settings,
        keybinds,
    }
    internal CurrentMenu menuState;
        internal void OnEscapePress(CurrentMenu menuState, bool rebinding)
    {
        switch (menuState)
        {
            case CurrentMenu.none:
                menuState = CurrentMenu.pause;
                //PauseGame();
                break;
            case CurrentMenu.pause:
                menuState = CurrentMenu.none;
                //ResumeGame();
                break;
            case CurrentMenu.settings:
                menuState = CurrentMenu.pause;
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
    private void OpenMenu(CurrentMenu menu)
    {
        switch (menu)
        {
            case CurrentMenu.none:
                currentMenu.SetActive(false);
                break;
            case CurrentMenu.pause:
                currentMenu.SetActive(true);
                settingsMenu.SetActive(false);
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
