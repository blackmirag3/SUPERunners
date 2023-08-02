using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class GunFireAndInputDeviceTest : InputTestFixture
{
    PlayerInput input = null;
    GameObject gun = null;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        yield return null;
        
        SceneManager.LoadScene("Level0");
        
        yield return null;

        gun = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Tests/Test Infrastructure/Test Prefabs/TestGun.prefab"));
        input = InputManager.instance.PlayerInput;
        GunFire gunFire = gun.GetComponent<GunFire>();
        gunFire.enabled = true;
        yield return null;
    }

    [UnityTest]
    public IEnumerator InputDeviceTest()
    {
        yield return null;

        Assert.That(input.inputIsActive, Is.EqualTo(true));
        Assert.That(input.defaultControlScheme, Is.EqualTo("KBM"));
        yield return null;
    }

    [UnityTest]
    public IEnumerator GunFireTestWithEnumeratorPasses()
    {
        GunFire gunFire = gun.GetComponent<GunFire>();
        yield return null;
        var mouse = InputSystem.AddDevice<Mouse>();
        gunFire.enabled = true;
        Press(mouse.leftButton);
        yield return null;
        Assert.IsTrue(gunFire.allowInvoke);
        Assert.IsTrue(mouse.leftButton.isPressed);
        yield return null;
        Release(mouse.leftButton);
        Assert.That(input.actions["Fire"].phase, Is.EqualTo(InputActionPhase.Waiting));
        yield return null; 
    }
    
    [UnityTest]
    public IEnumerator GunShootTest()
    {
        var mouse = InputSystem.AddDevice<Mouse>();
        GameObject bullet = gun.GetComponent<GunFire>().bullet;
        FireLogic gunFireTest = new FireLogic(bullet, new Vector3(0f, 50f, 0f), 1);

        InputAction fire = new InputAction("fireTest", binding: "<Mouse>/leftButton", interactions: "Hold");
        fire.Enable();
        fire.performed += gunFireTest.Fire;
        fire.canceled += gunFireTest.StopFire;

        
        yield return null;
        // Fire one bullet
        Press(mouse.leftButton);
        currentTime = 0.5;
        InputSystem.Update();
        Assert.IsTrue(gunFireTest.Shooting);
        BulletScript newBullet = GameObject.FindAnyObjectByType<BulletScript>();
        Assert.IsNotNull(newBullet);
        Assert.That(fire.phase, Is.EqualTo(InputActionPhase.Performed));
        Release(mouse.leftButton);
        InputSystem.Update();
        Assert.IsTrue(!gunFireTest.Shooting);
        Assert.That(fire.phase, Is.EqualTo(InputActionPhase.Waiting));

        // Ammo empty
        Press(mouse.leftButton);
        currentTime = 1.1;
        InputSystem.Update();
        Assert.IsTrue(!gunFireTest.Shooting);
        Assert.That(fire.phase, Is.EqualTo(InputActionPhase.Performed));
        Release(mouse.leftButton);
        InputSystem.Update();

        Assert.IsTrue(!gunFireTest.Shooting);
        yield return null;

        fire.Disable();
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        //input.actions.Disable();
        GameObject.Destroy(gun);
        yield return null;
    }
}
