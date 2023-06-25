using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]

public class GunData : ScriptableObject
{

    [Header("Info")]
    public new string name;

    [Header("Shooting")]
    public float shootForce;
    public float damage;
    public float maxDist;
    public float spread; //timeBetweenBullets;

    [Header("Ammo")]
    public int bulletsPerShot;
    public int magSize;
    public float fireRate;
    public bool isAuto;
    /*
    [Header("Enemy Stats")]
    //TODO add max and min values and randomiser to shooting functions (EnemyGun)
    public float enemyInaccuracy;
    public float enemyMaxShotDelay;
    public float enemyMinShotDelay;
    public int enemyMaxBurstSize;
    public float enemyBulletVelocity;
    public float enemyFirerate;
    */
}
