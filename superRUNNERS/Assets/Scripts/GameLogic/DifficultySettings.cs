using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Difficulty Settings", menuName = "Settings/Difficulty Settings")]
public class DifficultySettings : ScriptableObject
{
    public Difficulty difficulty;
    public float enemySpeedMultipliers;

    [Header("Player")]
    public float playerHealth;

    [Header("Game")]
    public int startEnemySpawn;
    public int maxEnemySpawn;

    [Header("Enemy")]
    public float enemySpeed;
    public float enemyUnarmedSpeed;
    public float maxEnemySpeed;
    public bool isAggro;
    public float enemyBulletVelocity;
}

[System.Serializable]
public enum Difficulty
{
    easy,
    normal,
    hard,
}
