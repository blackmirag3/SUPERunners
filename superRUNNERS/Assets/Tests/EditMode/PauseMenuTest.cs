using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

public class PauseMenuTest
{
    private PauseMenu pauseMenu;
    private MenuBuilder builder;
    private GameObject menu;
    public GameObject CreateMenu()
    {
        GameObject menuToCreate = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Tests/Test Infrastructure/Test Prefabs/TestMenu.prefab"));
        pauseMenu = menuToCreate.GetComponent<PauseMenu>();
        builder = menuToCreate.GetComponent<MenuBuilder>();
        builder.AsPause();
        return menuToCreate;
    }

    [Test]
    public void OnEscInGameOpenPause()
    {
        menu = CreateMenu();
        builder.Build();
        pauseMenu.menuState = PauseMenu.CurrentMenu.none;

        pauseMenu.OnEscapePress();
        Assert.IsTrue(pauseMenu.currentMenu.activeSelf == true);
        Assert.IsTrue(pauseMenu.settingsMenu.activeSelf == false);
        Assert.IsTrue(pauseMenu.keybindMenu.activeSelf == false);
    }

    [Test]
    public void OnEscInPauseOpenGame()
    {
        menu = CreateMenu();
        builder.OpenPause();
        builder.Build();
        pauseMenu.menuState = PauseMenu.CurrentMenu.pause;

        pauseMenu.OnEscapePress();
        Assert.IsTrue(pauseMenu.currentMenu.activeSelf == false);
        Assert.IsTrue(pauseMenu.settingsMenu.activeSelf == false);
        Assert.IsTrue(pauseMenu.keybindMenu.activeSelf == false);
    }

    [Test]
    public void OnEscInSettingsOpenPause()
    {
        menu = CreateMenu();
        builder.OpenSettings();
        builder.Build();
        pauseMenu.menuState = PauseMenu.CurrentMenu.settings;

        pauseMenu.OnEscapePress();
        Assert.IsTrue(pauseMenu.currentMenu.activeSelf == true);
        Assert.IsTrue(pauseMenu.settingsMenu.activeSelf == false);
        Assert.IsTrue(pauseMenu.keybindMenu.activeSelf == false);
    }

    [Test]
    public void OnEscInKeybindsOpenSettings()
    {
        menu = CreateMenu();
        builder.OpenKeyBindings();
        builder.Build();
        pauseMenu.menuState = PauseMenu.CurrentMenu.keybinds;

        pauseMenu.OnEscapePress();
        Assert.IsTrue(pauseMenu.currentMenu.activeSelf == false);
        Assert.IsTrue(pauseMenu.settingsMenu.activeSelf == true);
        Assert.IsTrue(pauseMenu.keybindMenu.activeSelf == false);
    }

    [Test]
    public void OnEscDuringRebindingDonothing()
    {
        menu = CreateMenu();
        builder.OpenKeyBindings();
        builder.Build();
        pauseMenu.menuState = PauseMenu.CurrentMenu.keybinds;
        pauseMenu.RebindingActive();

        pauseMenu.OnEscapePress();
        Assert.IsTrue(pauseMenu.currentMenu.activeSelf == false);
        Assert.IsTrue(pauseMenu.settingsMenu.activeSelf == false);
        Assert.IsTrue(pauseMenu.keybindMenu.activeSelf == true);
    }
}
