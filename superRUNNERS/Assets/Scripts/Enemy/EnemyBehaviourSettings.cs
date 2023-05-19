using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Behaviour Settings")]
public class EnemyBehaviourSettings : ScriptableObject
{
    [Header("Settings")]
    public bool isIdle;
    public bool isDead;
    public bool hasReachedPlayer;
    public float enemyHealth;
    public float enemySpeed;
    public float stoppingDistance;
    //public float shootingDelay; //TODO change to aggression
}
