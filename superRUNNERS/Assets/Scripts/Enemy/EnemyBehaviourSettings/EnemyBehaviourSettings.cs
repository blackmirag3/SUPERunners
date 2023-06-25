using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Behaviour Settings", menuName = "Settings/Enemy Behaviour Settings")]
public class EnemyBehaviourSettings : ScriptableObject
{
    [Header("States")]
    public bool isAggro;
    public bool isDead;
    public bool hasReachedPlayer;

    [Header("Base stats")]
    public float health;
    public float aggroDistance;

    [Header("Armed stats")]
    public float armedSpeed;
    public float armedStoppingDistance;
    public float enemyInaccuracy;
    public float enemyBulletVelocity;
    public float fireRate;
    public float maxShotDelay;
    public float minShotDelay;
    public int maxBurstSize;
    public int bulletsPerShot;

    [Header("Unarmed stats")]
    public float unarmedSpeed;
    public float unarmedStoppingDistance;
    public float minPunchDelay;
    public float maxPunchDelay;
    //public float shootingDelay; //TODO change to aggression
}
