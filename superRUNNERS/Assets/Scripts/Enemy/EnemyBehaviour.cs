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
    public AudioSource enemyHitSound;

    public EnemyBehaviourSettings settings;

    private float stoppingDistance;
    private float enemySpeed;
    public bool isDead;
    public float despawnTime;

    private bool isAggro;
    private bool hasReachedPlayer;
    private float enemyHealth;
    public bool recentHit = false;
    private float aggroDistance;


    public float meleeSpeed;
    public float meleeDist;
    private bool isArmed;

    //shooting
    private float currentShotTimer = 0;
    private float maxShotDelay;
    private float minShotDelay;
    private float maxBurstSize;
    private float bulletsShot;
    private float fireRate;
    private float currentBurstSize;
    //private float bulletsPerShot;

    public GameEvent onEnemyDeath;

    [SerializeField]
    private float lineOfSightDist;
    [SerializeField]
    private LayerMask layerMasks;

    //TODO
    //variation in firing
    //check aggro function
    //Enemy attack distance

    public void Damage(float damage)
    {
        enemyHitSound.Play();
        enemy.isStopped = true;
        enemyHealth -= damage;
        if (isArmed)
        {
            gunDrop.enabled = true;
            isArmed = false;
            anim.SetBool("isArmed", isArmed);
        }
        Invoke(nameof(ResetHit), 0.5f); 
        if (enemyHealth <= 0 && !isDead)
        {
            isDead = true;
            onEnemyDeath.CallEvent(this, null);
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

        //from enemyGun.gunData
        maxShotDelay = enemyGun.maxShotDelay;
        minShotDelay = maxShotDelay - enemyGun.minShotDelay;
        fireRate = enemyGun.fireRate;
        maxBurstSize = enemyGun.maxBurstSize;
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
        Vector3 playerDir = (player.position - transform.position).normalized;
        if (Physics.SphereCast(transform.position, 0.1f,playerDir, out RaycastHit hit, lineOfSightDist, layerMasks, QueryTriggerInteraction.Ignore))
        {
            return hit.collider.GetComponent<IDamageable>() != null;
        }

        return false;
    }

    public void EnemyShoot()
    {
        if (currentShotTimer > maxShotDelay && CheckLOS())
        {
            currentShotTimer = Random.Range(0, minShotDelay);
            currentBurstSize = Random.Range(0, maxBurstSize);
            ShootOneBurst();
        }
        currentShotTimer += Time.deltaTime;
    }

    private void ShootOneBurst()
    {
        if (enemyGun != null)
            enemyGun.Shoot();
        anim.Play("Additive Layer.enemyShoot", -1, 0);  
        bulletsShot += 1f;

        if (bulletsShot < currentBurstSize)
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
        isArmed = (enemyGun == null) ? false : true;
        anim.SetBool("isArmed", isArmed);
    }

    private void EnableEnemyMelee()
    {
        enemy.speed = meleeSpeed;
        enemy.stoppingDistance = meleeDist;
    }

}
        

