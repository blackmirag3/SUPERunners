using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandTrigger : MonoBehaviour
{
    [SerializeField]
    private string playerTag = "Player";
    [SerializeField]
    private float damage;
    private void Start()
    {
        Physics.IgnoreCollision(GetComponentInParent<Collider>(), GetComponent<Collider>());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            IDamageable player = other.GetComponent<IDamageable>();
            if (player != null)
            {
                player.Damage(damage);
            }
            //Debug.Log("Player hit detected");
        }
            
    }
}
