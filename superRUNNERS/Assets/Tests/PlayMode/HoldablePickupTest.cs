using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.SceneManagement;

public class HoldablePickupTest
{
    GameObject hand;
    GameObject gun;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("Level0");
        yield return null;

        hand = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Tests/Test Infrastructure/Test Prefabs/Empty.prefab"));
        gun = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Tests/Test Infrastructure/Test Prefabs/TestGun.prefab"));

        var pickup = gun.GetComponent<GunPickup>();
        pickup.Pickup(hand.GetComponent<Transform>());

        yield return null;
    }

    [UnityTest]
    public IEnumerator HoldableIsChildOfHand()
    {
        Assert.AreEqual(gun.transform.parent, hand.transform);
        yield return null;
    }

    [UnityTest]
    public IEnumerator HoldableIsEnabled()
    {
        Assert.IsTrue(gun.GetComponent<GunPickup>().isActiveAndEnabled == true);
        yield return null;
    }

    [UnityTest]
    public IEnumerator HoldableIsNotBreakable ()
    {
        var gunBreak = gun.GetComponentInChildren<GunBreak>();
        Assert.IsTrue(gunBreak.isActiveAndEnabled == false);
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        GameObject.Destroy(hand);
        GameObject.Destroy(gun);
        yield return null;
    }
}
