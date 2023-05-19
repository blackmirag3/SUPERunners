using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour, IDamageable
{
    private NavMeshAgent enemy;
    public Transform player;
    public Animator anim;

    public EnemyBehaviourSettings settings;

    private float stoppingDistance;
    private float enemySpeed;
    public EnemyGun enemyGun;
    public OnDeath death;

    public bool isDead;
    private bool isIdle;
    private bool hasReachedPlayer;
    private float enemyHealth;
    public bool recentHit = false;
    private float currentShotTimer;
    private float timePerShot;

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
        if (currentShotTimer > timePerShot && HasLOS())
        {
            enemyGun.FireBullet();
            anim.Play("Upper Body.Pistol Shoot", -1, 0);
            currentShotTimer = 0;
            Debug.Log(currentShotTimer);
        }

        currentShotTimer += Time.deltaTime;
    }

    private void InitialiseSettings()
    {
        isIdle = settings.isIdle;
        isDead = settings.isDead;
        hasReachedPlayer = settings.hasReachedPlayer;
        enemyHealth = settings.enemyHealth;
        enemySpeed = settings.enemySpeed;
        stoppingDistance = settings.stoppingDistance;
        timePerShot = settings.timePerShot;
    }

    private void Start()
    {
        InitialiseSettings();
        enemy = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemy.stoppingDistance = stoppingDistance;
        death.enabled = false;
        enemy.speed = enemySpeed;
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
        

