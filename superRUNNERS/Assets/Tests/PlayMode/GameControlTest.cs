using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class GameControlTest
{
    private PlayerHealth health;
    private GameControl control;
    private PauseMenu pause;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("Arena");
        yield return null;
        GameObject obj = GameObject.Find("GameLogic");
        control = obj.GetComponent<GameControl>();
        GameObject menu = GameObject.Find("Menu");
        pause = menu.GetComponent<PauseMenu>();
    }

    [UnityTest]
    public IEnumerator RestartGameLoadsArenaScene()
    {
        control.RestartGame();
        yield return null;
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        Assert.AreEqual("Arena", sceneName);
        yield return null;
    }

    [UnityTest]
    public IEnumerator QuitGameLoadsMainMenuScene()
    {
        pause.BackToMainMenu();
        yield return null;
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        Assert.AreEqual("MainMenu", sceneName);
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        yield return null;
    }
}
