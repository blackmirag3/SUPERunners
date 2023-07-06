using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private DifficultySettings diff;

    [SerializeField] private float playerHealth;
    [SerializeField] private float invulnerableDuration;
    private bool isInvulnerable;

    [SerializeField] private AudioSource playerHurtSound;

    [SerializeField] private GameEvent playerDeath;

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
        if (!isInvulnerable)
        {
            isInvulnerable = true;
            playerHealth -= damage;
            if (playerHurtSound != null)
            {
                playerHurtSound.Play();
            }
            StartCoroutine(DeactivateInvulnerability(invulnerableDuration));

            Debug.Log($"Health left: {playerHealth}");
            if (playerHealth <= 0)
            {
                // stop game
                playerDeath.CallEvent(this, null);
            }
        }
        else Debug.Log("player has stopwatch active");
    }

    private IEnumerator DeactivateInvulnerability(float duration)
    {
        yield return new WaitForSeconds(duration);
        isInvulnerable = false;
    }
}
