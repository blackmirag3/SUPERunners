using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletTrigger : MonoBehaviour
{
    [SerializeField] private EnemyBullet bullet;
    [SerializeField]
    private string PlayerTag = "Player";
    [SerializeField]
    private float damage;
    private Vector3 initialPos;
    private void Start()
    {
        initialPos = GetComponentInParent<Transform>().position;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            IDamageable Player = other.GetComponent<IDamageable>();
            DamageIndicatorManager damageIndicatorManager = other.GetComponentInParent<DamageIndicatorManager>();
            if (Player != null)
            {
                Player.Damage(damage);
            }
            if (damageIndicatorManager != null)
            {
                damageIndicatorManager.SpawnIndicator(initialPos);
            }
            Destroy(transform.parent.gameObject);
            //Debug.Log("Player hit detected");
        }
            
    }
}
