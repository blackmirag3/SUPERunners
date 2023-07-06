using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float damageToKill;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            IDamageable player = other.GetComponent<IDamageable>();
            if (player != null)
            {
                player.Damage(damageToKill);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            IDamageable player = other.GetComponent<IDamageable>();
            if (player != null)
            {
                player.Damage(damageToKill);
            }
        }
    }
}
