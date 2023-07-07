using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    [SerializeField] private string PlayerTag = "Player";
    [SerializeField] private float damageToKill;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            IDamageable Player = other.GetComponent<IDamageable>();
            if (Player != null)
            {
                Player.Damage(damageToKill);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            IDamageable Player = other.GetComponent<IDamageable>();
            if (Player != null)
            {
                Player.Damage(damageToKill);
            }
        }
    }
}
