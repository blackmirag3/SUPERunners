using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject currentMenu;
    [SerializeField]
    private GameObject settingsMenu;
    [SerializeField]
    private GameObject keybindMenu;
    public bool gameIsPaused;

    private KeyCode escapeKey = KeyCode.Escape;
    private bool isOtherMenu;

    private bool rebinding;

    public GameEvent onPause;

    private PlayerInput inputs;

    private void Awake()
    {
        inputs = InputManager.instance.PlayerInput;
    }

    private enum CurrentMenu
    {
        none,
        pause,
        settings,
        keybinds,
    }
    private CurrentMenu menuState;

    private void Start()
    {
        menuState = CurrentMenu.none;
        gameIsPaused = false;
        rebinding = false;
    }

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
            case CurrentMenu.none:
                menuState = CurrentMenu.pause;
                PauseGame();
                break;
            case CurrentMenu.pause:
                menuState = CurrentMenu.none;
                ResumeGame();
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

    public void StartGame()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    public void CloseSettings()
    {
        menuState = CurrentMenu.pause;
        OpenMenu(menuState);
        PlayerPrefs.Save();
    }

    public void PauseGame()
    {
        menuState = CurrentMenu.pause;
        OpenMenu(menuState);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        inputs.actions.Disable();
        onPause.CallEvent(this, true);
    }

    public void ResumeGame()
    {
        menuState = CurrentMenu.none;
        OpenMenu(menuState);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        inputs.actions.Enable();
        onPause.CallEvent(this, false);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;

        if (DifficultySelector.instance != null)
        {
            
            Destroy(DifficultySelector.instance.gameObject);
        }

        SceneManager.LoadScene(0);
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
