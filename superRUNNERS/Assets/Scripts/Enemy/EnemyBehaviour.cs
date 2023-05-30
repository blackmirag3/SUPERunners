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
    public GunDrop gunDrop;

    public EnemyBehaviourSettings settings;

    private float stoppingDistance;
    private float enemySpeed;
    public bool isDead;
    public float despawnTime;
    public KillCounter killCounter;

    private bool isAggro;
    private bool hasReachedPlayer;
    private float enemyHealth;
    public bool recentHit = false;
    private float aggroDistance;


    public float meleeSpeed;
    public float meleeDist;
    private bool isArmed;

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
        enemy.isStopped = true;
        enemyHealth -= damage;
        if (isArmed)
        {
            gunDrop.enabled = true;
            isArmed = false;
        }
        Invoke(nameof(ResetHit), 0.5f); 
        if (enemyHealth <= 0 && !isDead)
        {
            isDead = true;
            killCounter.leftToKill--;
            Destroy(gameObject, despawnTime);
        }
        else if (!isDead)
        {
            EnableEnemyMelee();
        }
    }

    private void Start()
    {
        CheckedArmed();
        InitialiseSettings();
        enemy = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemy.stoppingDistance = stoppingDistance;
        gunDrop.enabled = false;
        enemy.speed = enemySpeed;

        //from enemyGun.gunData (optimise potential)
        shotDelay = enemyGun.shotDelay;
        fireRate = enemyGun.fireRate;
        burstSize = enemyGun.burstSize;
        //bulletsPerShot = enemyGun.bulletsPerShot;

        anim.SetBool("isAggro", isAggro);
        anim.SetBool("hasReachedPlayer", hasReachedPlayer);
    }

    private void Update()
    {
        if (isDead)

        {
            anim.enabled = false;
            //anim.SetBool("isDead", isDead);
        }

        else if (!isAggro)
        {
            if (!isAggro)
            {
                isAggro = CheckAggro();
                anim.SetBool("isAggro", isAggro);
            }
        }

        else if (!recentHit)
        {
            EnemyChase();
            if (isArmed)
            {
                EnemyShoot();
            }
        }
    }

    private void ResetHit()
    {
        recentHit = false;
        enemy.isStopped = false;
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

    private bool CheckLOS()
    {
        NavMeshHit hit;
        return (!enemy.Raycast(player.position, out hit)); //if no obstacle between enemy LOS and player
    }

    public void EnemyShoot()
    {
        if (currentShotTimer > shotDelay && CheckLOS())
        {
            ShootOneBurst();
            currentShotTimer = 0;

            //currentShotTimer = Random.Range(0, shotDelay/);
            // Random.Range(0, currentBurstCounter);
        }
        currentShotTimer += Time.deltaTime;
    }

    private void ShootOneBurst()
    {
        enemyGun.Shoot();
        anim.Play("Upper Body.Pistol Shoot", -1, 0);
        bulletsShot += 1f;

        if (bulletsShot < burstSize)
        {
            Invoke("ShootOneBurst", fireRate);
        }
        else bulletsShot = 0;
    }

    private bool CheckAggro()
    {

        float distance = Vector3.Distance(transform.position, player.position);
        return ((distance <= aggroDistance) && CheckLOS());
    }

    private void InitialiseSettings()
    {
        isAggro = settings.isAggro;
        isDead = settings.isDead;
        hasReachedPlayer = settings.hasReachedPlayer;
        enemyHealth = settings.enemyHealth;
        enemySpeed = settings.enemySpeed;
        stoppingDistance = settings.stoppingDistance;
        aggroDistance = settings.aggroDistance;
    }

    private void CheckedArmed()
    {
        if (enemyGun == null)
        {
            isArmed = false;
            return;
        }
        isArmed = true;
    }

    private void EnableEnemyMelee()
    {
        enemy.speed = meleeSpeed;
        enemy.stoppingDistance = meleeDist;
    }

}
        

