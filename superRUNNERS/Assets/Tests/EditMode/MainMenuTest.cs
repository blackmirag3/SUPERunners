using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

public class MainMenuTest
{
    private MainMenu mainMenu;
    private MenuBuilder builder;
    private GameObject menu;
    public GameObject CreateMenu()
    {
        GameObject menuToCreate = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Tests/Test Infrastructure/Test Prefabs/TestMenu.prefab"));
        mainMenu = menuToCreate.GetComponent<MainMenu>();
        builder = menuToCreate.GetComponent<MenuBuilder>();
        builder.AsMain();
        return menuToCreate;
    }
    // A Test behaves as an ordinary method
    [Test]
    public void OnEscInMainDoNothing()
    {
        menu = CreateMenu();
        builder.OpenMain();
        builder.Build();
        mainMenu.menuState = MainMenu.CurrentMenu.main;

        mainMenu.OnEscapePress();
        Assert.IsTrue(mainMenu.currentMenu.activeSelf == true);
        Assert.IsTrue(mainMenu.settingsMenu.activeSelf == false);
        Assert.IsTrue(mainMenu.diffSelector.activeSelf == false);
        Assert.IsTrue(mainMenu.keybindMenu.activeSelf == false);
    }

    [Test]
    public void OnEscInPlayOpenMain()
    {
        menu = CreateMenu();
        builder.OpenDifficulty();
        builder.Build();
        mainMenu.menuState = MainMenu.CurrentMenu.play;

        mainMenu.OnEscapePress();
        Assert.IsTrue(mainMenu.currentMenu.activeSelf == true);
        Assert.IsTrue(mainMenu.settingsMenu.activeSelf == false);
        Assert.IsTrue(mainMenu.diffSelector.activeSelf == false);
        Assert.IsTrue(mainMenu.keybindMenu.activeSelf == false);
    }

    [Test]
    public void OnEscInSettingsOpenMain()
    {
        menu = CreateMenu();
        builder.OpenSettings();
        builder.Build();
        mainMenu.menuState = MainMenu.CurrentMenu.settings;

        mainMenu.OnEscapePress();
        Assert.IsTrue(mainMenu.currentMenu.activeSelf == true);
        Assert.IsTrue(mainMenu.settingsMenu.activeSelf == false);
        Assert.IsTrue(mainMenu.diffSelector.activeSelf == false);
        Assert.IsTrue(mainMenu.keybindMenu.activeSelf == false);
    }

    [Test]
    public void OnEscInKeybindsOpenSettings()
    {
        menu = CreateMenu();
        builder.OpenKeyBindings();
        builder.Build();
        mainMenu.menuState = MainMenu.CurrentMenu.keybinds;

        mainMenu.OnEscapePress();
        Assert.IsTrue(mainMenu.currentMenu.activeSelf == false);
        Assert.IsTrue(mainMenu.settingsMenu.activeSelf == true);
        Assert.IsTrue(mainMenu.diffSelector.activeSelf == false);
        Assert.IsTrue(mainMenu.keybindMenu.activeSelf == false);
    }

    [Test]
    public void OnEscDuringRebindingDoNothing()
    {
        menu = CreateMenu();
        builder.OpenKeyBindings();
        builder.Build();
        mainMenu.menuState = MainMenu.CurrentMenu.keybinds;
        mainMenu.RebindingActive();

        mainMenu.OnEscapePress();
        Assert.IsTrue(mainMenu.currentMenu.activeSelf == false);
        Assert.IsTrue(mainMenu.settingsMenu.activeSelf == false);
        Assert.IsTrue(mainMenu.diffSelector.activeSelf == false);
        Assert.IsTrue(mainMenu.keybindMenu.activeSelf == true);
    }
}
