using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovementTest : InputTestFixture
{
    private Keyboard keyboard;
    private GameObject player;

    public override void Setup()
    {
        base.Setup();
        keyboard = InputSystem.AddDevice<Keyboard>();
    }
    [UnitySetUp]
    public IEnumerator SceneSetup()
    {
        Setup();
        SceneManager.LoadScene("Arena");
        yield return null;
        player = GameObject.Find("Player/PlayerBody");
    }

    [UnityTest]
    public IEnumerator TestWASDInput()
    {
        player = GameObject.Find("Player/PlayerBody");
        Vector3 prevTransform = player.transform.position;
        Press(keyboard.aKey);
        Press(keyboard.dKey);
        yield return new WaitForEndOfFrame();
        Assert.IsTrue(prevTransform.z != player.transform.position.z);
        Assert.IsTrue(prevTransform.x != player.transform.position.x);
        prevTransform = player.transform.position;
        yield return null;
    }

    public override void TearDown()
    {
        base.TearDown();
    }
}
