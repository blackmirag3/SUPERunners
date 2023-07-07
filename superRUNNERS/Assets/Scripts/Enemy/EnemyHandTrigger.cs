using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandTrigger : MonoBehaviour
{
    [SerializeField]
    private string PlayerTag = "Player";
    [SerializeField] private Transform enemy;
    [SerializeField] private float damage;

    private void Awake()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), GetComponentInParent<Collider>());
    }

    private void OnTriggerEnter(Collider col)
    {
        
        if (col.CompareTag(PlayerTag))
        {
            Debug.Log($"Hand collided ({col})");
            IDamageable Player = col.gameObject.GetComponent<IDamageable>();
            DamageIndicatorManager damageIndicatorManager = col.GetComponentInParent<DamageIndicatorManager>();
            if (Player != null)
            {
                Player.Damage(damage);
                Debug.Log("Enemy has punched Player");
                GetComponent<Collider>().enabled = false;
            }
            if (damageIndicatorManager != null)
            {
                damageIndicatorManager.SpawnIndicator(enemy.position);
            }
        }
    }
}
