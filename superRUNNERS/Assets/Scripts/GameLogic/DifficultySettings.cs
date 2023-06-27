using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Difficulty Settings", menuName = "Settings/Difficulty Settings")]
public class DifficultySettings : ScriptableObject
{
    public Difficulty difficulty;

    [Header("Player")]
    public float playerHealth;

    [Header("Game")]
    public int startEnemySpawn;

    [Header("Enemy")]
    public float enemySpeed;
    public float enemyUnarmedSpeed;
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
