using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour, IDamageable
{
    private NavMeshAgent enemy;
    public Transform player;
    public Animator anim;
    private bool isIdle;
    private bool hasReachedPlayer;
    public bool isDead;
    public float stoppingDistance = 6f;
    public float enemySpeed;
    public EnemyGun enemyGun;
    public OnDeath death;

    private float enemyHealth;
    public bool recentHit = false;

    private float currentShotTimer;
    public float shotTimer;

    //TODO
    //check aggro function
    //Enemy attack distance

    public void Damage(float damage)
    {
        enemyHealth -= damage;
        Invoke(nameof(ResetHit), 0.25f);
        if (enemyHealth <= 0)
        {
            isDead = true;
        }
    }

    private void ResetHit()
    {
        recentHit = false;
    }

    private void EnemyChase()
    {
        enemy.SetDestination(player.position); //chase player
        hasReachedPlayer = (enemy.remainingDistance <= enemy.stoppingDistance);
        anim.SetBool("hasReachedPlayer", hasReachedPlayer);

        if (hasReachedPlayer)
        {
            Vector3 dir = player.position - transform.position;
            Quaternion enemyFaceRotation = Quaternion.LookRotation(dir);
            float yRotation = enemyFaceRotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        }
    }

    private bool HasLOS()
    {
        bool hasLOS = false;
        NavMeshHit hit;

        if (!enemy.Raycast(player.position, out hit)) //if no obstacle between enemy LOS and player
        {
            hasLOS = true;
        }
        return hasLOS;
    }

    public void EnemyShoot()
    {
        if (currentShotTimer > shotTimer && HasLOS())
        {
            enemyGun.FireBullet();
            anim.Play("Upper Body.Pistol Shoot", -1, 0);
            currentShotTimer = 0;
            Debug.Log(currentShotTimer);
        }

        currentShotTimer += Time.deltaTime;
    }

    private void Start()
    {
        death.enabled = false;
        enemyHealth = 1f;
        enemy = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemy.stoppingDistance = stoppingDistance;
        isIdle = true;
        hasReachedPlayer = false;
        isDead = false;
        enemy.speed = enemySpeed;
        shotTimer = 3f;
    }

    private void Update()
    {
        if (isDead)
        {
            anim.enabled = false;
            death.enabled = true;
            //anim.SetBool("isDead", isDead);
        }

        else if (isIdle)
        {
            isIdle = false;
            anim.SetBool("isIdle", isIdle);
        }

        else
        {
            EnemyChase();
            EnemyShoot();
        }
    }

}
        

