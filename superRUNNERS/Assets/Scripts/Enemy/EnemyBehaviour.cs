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
    private bool recentMelee = false;


    private float unarmedSpeed;
    private float unarmedStoppingDistance;
    private bool isArmed;
    private bool isStaggered;
    private float minPunchDelay;
    private float maxPunchDelay;

    //shooting
    private float currentAttackTimer = 0;
    private float maxShotDelay;
    private float minShotDelay;
    private int maxBurstSize;
    private int bulletsShot;
    private float fireRate;
    private int currentBurstSize;
    //private float bulletsPerShot;

    public GameEvent onEnemyDeath;

    [SerializeField]
    private float lineOfSightDist;
    [SerializeField]
    private LayerMask layerMasks;

    [SerializeField]
    private Collider rightHand, leftHand;
    [SerializeField]
    private float punchColTime, punchResetTime;

    public void Damage(float damage)
    {
        enemyHitSound.Play();
        enemy.isStopped = true;
        isStaggered = true;
        enemyHealth -= damage;
        if (isArmed)
        {
            gunDrop.enabled = true;
            isArmed = false;
            anim.SetBool("isArmed", isArmed);
        }
        StartCoroutine(ResetHit()); 
        if (enemyHealth <= 0 && !isDead)
        {
            isDead = true;
            rightHand.enabled = false;
            leftHand.enabled = false;
            onEnemyDeath.CallEvent(this, null);
            Destroy(gameObject, despawnTime);
        }
        else if (!isDead)
        {
            EnableEnemyUnarmed();
            currentAttackTimer = 0;
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
        isDead = false;
        enemy.speed = enemySpeed;
        anim.SetBool("isAggro", isAggro);
        anim.SetBool("hasReachedPlayer", hasReachedPlayer);
        isStaggered = false;
    }

    private void Update()
    {
        if (isDead) //In Dead state
        {
            anim.enabled = false;
            //anim.SetBool("isDead", isDead);
        }

        else if (!isAggro) //In Idle state
        {
            if (!isAggro)
            {
                isAggro = CheckAggro();
                anim.SetBool("isAggro", isAggro);
            }
        }

        else if (!recentHit && !isStaggered) //In Aggro state
        {
            EnemyChase();

            if (isArmed) //In Armed sub-state
            {
                EnemyShoot();
            }

            else //In Unarmed sub-state
            {
                if (hasReachedPlayer)
                    EnemyPunch();
            }
        }
    }

    private IEnumerator ResetHit()
    {
        yield return new WaitForSeconds(0.3f);
        recentHit = false;
        yield return new WaitForSeconds(0.2f);
        enemy.isStopped = false;
        isStaggered = false;
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
        if (Physics.SphereCast(transform.position, 0.1f, playerDir, out RaycastHit hit, lineOfSightDist, layerMasks, QueryTriggerInteraction.Ignore))
        {
            return hit.collider.GetComponent<IDamageable>() != null;
        }
        return false;
    }

    private void EnemyShoot()
    {
        if (currentAttackTimer > maxShotDelay && CheckLOS())
        {
            currentAttackTimer = Random.Range(0, maxShotDelay - minShotDelay);
            currentBurstSize = Random.Range(1, maxBurstSize + 1);
            ShootOneBurst();
        }
        currentAttackTimer += Time.deltaTime;
    }

    private void ShootOneBurst()
    {
        if (enemyGun != null)
            enemyGun.Shoot();
        anim.Play("Additive Layer.Shoot", -1, 0);  
        bulletsShot += 1;

        if (bulletsShot < currentBurstSize)
        {
            Invoke("ShootOneBurst", fireRate);
        }
        else bulletsShot = 0;
    }

    private void EnemyPunch()
    {
        if (!recentMelee)
        {
            enemy.isStopped = true;
            anim.Play("Additive Layer.HeavyJab", -1, 0);
            anim.SetInteger("UnarmedIndex", Random.Range(0, 4));
            recentMelee = true;

            StartCoroutine(PunchRoutine());
        }
    }

    private IEnumerator PunchRoutine()
    {
        yield return new WaitForSeconds(punchResetTime);
        rightHand.enabled = true;
        leftHand.enabled = true;
        yield return new WaitForSeconds(punchColTime);
        rightHand.enabled = false;
        leftHand.enabled = false;
        yield return new WaitForSeconds(punchResetTime);
        enemy.isStopped = false;
        recentMelee = false;
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
        enemyHealth = settings.health;
        enemySpeed = settings.armedSpeed;
        stoppingDistance = settings.armedStoppingDistance;
        aggroDistance = settings.aggroDistance;
        maxShotDelay = settings.maxShotDelay;
        minShotDelay = settings.minShotDelay;
        fireRate = settings.fireRate;
        maxBurstSize = settings.maxBurstSize;
        unarmedStoppingDistance = settings.unarmedStoppingDistance;
        unarmedSpeed = settings.unarmedSpeed;
        minPunchDelay = settings.minPunchDelay;
        maxPunchDelay = settings.maxPunchDelay;
    }

    private void CheckedArmed()
    {
        isArmed = (enemyGun == null) ? false : true;
        anim.SetBool("isArmed", isArmed);
    }

    private void EnableEnemyUnarmed()
    {
        enemy.speed = unarmedSpeed;
        enemy.stoppingDistance = unarmedStoppingDistance;
    }
}
        

