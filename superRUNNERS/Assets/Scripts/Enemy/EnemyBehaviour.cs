using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour, IDamageable
{
    private NavMeshAgent enemy;
    public Transform player;
    public Animator anim;
    public EnemyGun enemyGun;
    public OnDeath death;

    public EnemyBehaviourSettings settings;

    private float stoppingDistance;
    private float enemySpeed;
    public bool isDead;
    private bool isIdle;
    private bool hasReachedPlayer;
    private float enemyHealth;
    public bool recentHit = false;

    //shooting
    private float currentShotTimer;
    private float shotDelay;
    private float burstSize;
    private float bulletsShot;
    private float fireRate;
    //private float bulletsPerShot;


    //TODO
    //variation in firing
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
        if (currentShotTimer > shotDelay && HasLOS())
        {
            ShootBurst();
            currentShotTimer = 0;

            //currentShotTimer = Random.Range(0, shotDelay/);
            // Random.Range(0, currentBurstCounter);
        }
        currentShotTimer += Time.deltaTime;
    }

    private void ShootBurst()
    {
        enemyGun.Shoot();
        anim.Play("Upper Body.Pistol Shoot", -1, 0);
        bulletsShot += 1f;

        if (bulletsShot < burstSize)
        {
            Invoke("ShootBurst", fireRate);
        }
        else bulletsShot = 0;
    }

    private void InitialiseSettings()
    {
        isIdle = settings.isIdle;
        isDead = settings.isDead;
        hasReachedPlayer = settings.hasReachedPlayer;
        enemyHealth = settings.enemyHealth;
        enemySpeed = settings.enemySpeed;
        stoppingDistance = settings.stoppingDistance;
    }

    private void Start()
    {
        InitialiseSettings();
        enemy = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemy.stoppingDistance = stoppingDistance;
        death.enabled = false;
        enemy.speed = enemySpeed;

        //from enemyGun.gunData (optimise potential)
        shotDelay = enemyGun.shotDelay;
        fireRate = enemyGun.fireRate;
        burstSize = enemyGun.burstSize;
        //bulletsPerShot = enemyGun.bulletsPerShot;
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
        

