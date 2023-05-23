using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionEnterDeath : MonoBehaviour
{
    public EnemyBehaviour enemy;
    public Animator anim;
    [SerializeField] private string bulletTag = "PlayerBullet";
    [SerializeField] private string gunTag = "Damageable";
    public Rigidbody rb;


    private void Awake()
    {
        enemy = GetComponentInParent<EnemyBehaviour>();
        anim = GetComponentInParent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!enemy.recentHit)
        {
            if (collision.gameObject.CompareTag(bulletTag))
            {
                anim.SetTrigger("enemyHit");
                enemy.recentHit = true;
                float damage = collision.gameObject.GetComponent<BulletScript>().damage;
                Debug.Log("Bullet hit detected");
                enemy.Damage(damage);
            }
            else if (collision.gameObject.CompareTag(gunTag))
            {
                anim.SetTrigger("enemyHit");
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
                anim.SetTrigger("enemyHit");
                enemy.recentHit = true;
                Debug.Log("Melee hit detected");
                enemy.Damage(0.5f);
            }
        }
    }

}
