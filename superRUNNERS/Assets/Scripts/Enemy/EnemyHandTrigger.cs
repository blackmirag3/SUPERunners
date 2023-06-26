using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandTrigger : MonoBehaviour
{
    [SerializeField]
    private string playerTag = "Player";
    [SerializeField]
    private float damage;

    private void Awake()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), GetComponentInParent<Collider>());
    }

    private void OnTriggerEnter(Collider col)
    {
        
        if (col.CompareTag(playerTag))
        {
            Debug.Log($"Hand collided ({col})");
            IDamageable player = col.gameObject.GetComponent<IDamageable>();
            if (player != null)
            {
                player.Damage(damage);
                Debug.Log("Enemy has punched player");
                GetComponent<Collider>().enabled = false;
            }
        }
    }

    /*
    private void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.collider);
        if (col.collider.CompareTag(playerTag))
        {
            Debug.Log("Hand collided (collision)");
            IDamageable player = col.gameObject.GetComponent<IDamageable>();
            if (player != null)
            {
                player.Damage(damage);
                Debug.Log("Enemy has punched player");
            }
        }
    }
    */
}
