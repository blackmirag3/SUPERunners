using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionEnterDeath : MonoBehaviour
{
    public EnemyBehaviour enemy;
    [SerializeField] private string bulletTag = "PlayerBullet";
    [SerializeField] private string gunTag = "Gun";

    private void OnCollisionEnter(Collision collision)
    {
        if (!enemy.recentHit)
        {
            if (collision.gameObject.CompareTag(bulletTag))
            {
                enemy.recentHit = true;
                float damage = collision.gameObject.GetComponent<BulletScript>().damage;
                Debug.Log("Bullet hit detected");
                enemy.Damage(damage);
            }
            else if (collision.gameObject.CompareTag(gunTag))
            {
                enemy.recentHit = true;
                Debug.Log("Gun hit detected");
                enemy.Damage(0.5f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enemy.recentHit)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                enemy.recentHit = true;
                Debug.Log("Melee hit detected");
                enemy.Damage(0.5f);
            }
        }
    }
}
