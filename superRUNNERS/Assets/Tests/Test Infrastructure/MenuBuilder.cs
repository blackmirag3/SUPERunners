using UnityEngine;

public class MenuBuilder: MonoBehaviour
{
    //[SerializeField] private GameObject menu;
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject keybind;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject difficulty;
    [SerializeField] private GameObject pause;
    private bool isMainMenu;
    private bool isPauseMenu;
    private bool isMainOpen;
    private bool isSettingsOpen;
    private bool isKeyBindingsOpen;
    private bool isDifficultyOpen;
    private bool isPauseOpen;

    public void AsMain()
    {
        isMainMenu = true;
    }

    public void AsPause()
    {
        isPauseMenu = true;
    }

    public void OpenMain()
    {
        isMainOpen = true;
    }

    public void OpenPause()
    {
        isPauseOpen = true;
    }
    public void OpenSettings()
    {
        isSettingsOpen = true;
    }

    public void OpenKeyBindings()
    {
        isKeyBindingsOpen = true;
    }

    public void OpenDifficulty()
    {
        isDifficultyOpen = true;
    }

    public void Build()
    {
        if (isMainMenu)
        {
        main.SetActive(isMainOpen);
        settings.SetActive(isSettingsOpen);
        difficulty.SetActive(isDifficultyOpen);
        keybind.SetActive(isKeyBindingsOpen);
        }
        else if (isPauseMenu)
        {
        pause.SetActive(isPauseOpen);
        settings.SetActive(isSettingsOpen);
        keybind.SetActive(isKeyBindingsOpen);
        }
    }
}
