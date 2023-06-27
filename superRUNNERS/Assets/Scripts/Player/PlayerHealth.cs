using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private DifficultySettings diff;

    [SerializeField] private float playerHealth;

    public GameEvent playerDeath;

    private void Awake()
    {
        if (DifficultySelector.instance != null)
        {
            diff = DifficultySelector.instance.selectedDifficulty;
        }

        playerHealth = diff.playerHealth;
    }

    public void Damage(float damage)
    {
        playerHealth -= damage;
        Debug.Log($"Health left: {playerHealth}");
        if (playerHealth <= 0)
        {
            // stop game
            playerDeath.CallEvent(this, null);
        }
    }
}
