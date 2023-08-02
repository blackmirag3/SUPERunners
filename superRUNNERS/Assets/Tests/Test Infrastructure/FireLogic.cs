using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireLogic
{
    public bool Shooting { get; private set; }

    private GameObject bullet;
    private Vector3 muzzlePosition;
    private int ammo;

    public FireLogic(GameObject newBullet, Vector3 shootPos, int newAmmo)
    {
        bullet = newBullet;
        muzzlePosition = shootPos;
        ammo = newAmmo;
        Shooting = false;
    }

    public void Fire(InputAction.CallbackContext ctx)
    {
        if (ammo > 0)
        {
            Shooting = true;
            ammo -= 1;
            GameObject.Instantiate(bullet, muzzlePosition, Quaternion.identity);
        }
    }

    public void StopFire(InputAction.CallbackContext ctx)
    {
        Shooting = false;
    }
}
