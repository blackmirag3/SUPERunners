using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicatorTrigger : MonoBehaviour
{
    [SerializeField] private string PlayerTag = "Player";
    [SerializeField] private Transform enemy;
    [SerializeField] private float damage;
    // Update is called once per frame
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag(PlayerTag))
        {
            DamageIndicatorManager damageIndicatorManager = col.GetComponentInParent<DamageIndicatorManager>();
            if (damageIndicatorManager != null)
            {
                damageIndicatorManager.SpawnIndicator(enemy.position);
            }
        }
    }
}
