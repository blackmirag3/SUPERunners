using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.SceneManagement;

public class HoldableThrowTest
{
    private GameObject gun;
    private GunPickup pickupScript;
    private float despawnTime;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("Level0");
        yield return null;

        //hand = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Tests/Test Infrastructure/Test Prefabs/Empty.prefab"));
        gun = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Tests/Test Infrastructure/Test Prefabs/TestGun.prefab"));
        pickupScript = gun.GetComponent<GunPickup>();
        despawnTime = pickupScript.despawnTime;
        Vector3 point = new Vector3(0, 100, 0);
        pickupScript.Throw(point);

        yield return null;
    }

    [UnityTest]
    public IEnumerator HoldableHasNoParent()
    {
        Assert.IsTrue(gun.transform.parent == null);
        yield return null;
    }

    [UnityTest]
    public IEnumerator HoldableIsDisabled()
    {
        Assert.IsTrue(!pickupScript.isGunEnabled);
        yield return null;
    }

    [UnityTest]
    public IEnumerator HoldableIsBreakable ()
    {
        Assert.IsTrue(pickupScript.isGunBreakEnabled);
        yield return null;
    }

    [UnityTest]
    public IEnumerator HoldableIsDestroyedAfterTime ()
    {
        yield return new WaitForSeconds(despawnTime);
        Assert.IsTrue(gun == null);
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        GameObject.Destroy(gun);
        yield return null;
    }
}
