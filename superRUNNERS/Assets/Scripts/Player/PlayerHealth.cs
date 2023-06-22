using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float playerHealth;

    public GameEvent playerDeath;

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
