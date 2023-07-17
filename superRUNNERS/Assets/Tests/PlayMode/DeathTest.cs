using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class HealthTest
{
    private PlayerHealth health;
    private GameObject deathscreen;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("Arena");
        yield return null;
        GameObject player = GameObject.Find("PlayerObj");
        health = player.GetComponent<PlayerHealth>();
    }

    [UnityTest]
    public IEnumerator DeathScreenOnNoHealth()
    {
        health.Damage(100);
        yield return null;
        deathscreen = GameObject.Find("GameLogic/GameOver");
        Assert.IsTrue(deathscreen.activeSelf);
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        yield return null;
    }
}
