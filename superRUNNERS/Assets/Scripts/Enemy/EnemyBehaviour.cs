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
    public float attackRange;
    public float enemySpeed;
    public EnemyGun enemyGun;
    public OnDeath death;

    private float enemyHealth;
    public bool recentHit = false;
    //TODO check dead function
    //bool checkDead?

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
        //Debug.Log("enemy has LOS: " + hasLOS);
        return hasLOS;
    }

    public void EnemyShoot() //called by animation event
    {
        if (HasLOS())
        {
            float inaccuracy = 1f; ////TODO add inaccuracy based on gun type
            Vector3 target = player.position - Random.Range(0, inaccuracy) * Vector3.up;
            enemyGun.FireBullet(); //requires enemyGun script on enemy gun
        }

    }
    //TODO throw gun on death function

    private void Start()
    {
        death.enabled = false;
        enemyHealth = 1f;
        enemy = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemy.stoppingDistance = attackRange;
        isIdle = true;
        hasReachedPlayer = false;
        isDead = false;
        enemy.speed = enemySpeed;
    }

    private void Update()
    {
        if (isDead) //dead state
        {
            anim.enabled = false;
            death.enabled = true;
            //anim.SetBool("isDead", isDead);
            //Debug.Log("enemy is dead:" + isDead);
        }

        else if (isIdle) //idle state
        {
            //TODO check aggro function (idle -> aggro)
            isIdle = false; //assumed aggro
            anim.SetBool("isIdle", isIdle);
        }

        else //aggro state
        {
            EnemyChase();
            //TODO different chase states
            if (HasLOS())
            {
            }

        }

        //Debug.Log("enemy is idle: " + isIdle);
        //Debug.Log("enemy has reached player: " + hasReachedPlayer);
    }

}
        

