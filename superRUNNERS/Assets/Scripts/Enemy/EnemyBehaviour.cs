using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
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

    //TODO check dead function
    //bool checkDead?

    void EnemyChase()
    {
        enemy.SetDestination(player.position); //chase player
        hasReachedPlayer = (enemy.remainingDistance <= enemy.stoppingDistance);
        anim.SetBool("hasReachedPlayer", hasReachedPlayer);
    }

    bool HasLOS()
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

    void EnemyShoot() //called by animation event
    {
        if (HasLOS())
        {
            float inaccuracy = 1f; ////TODO add inaccuracy based on gun type
            Vector3 target = player.position - Random.Range(0, inaccuracy) * Vector3.up;
            enemyGun.FireBullet(); //requires enemyGun script on enemy gun
        }

    }
    //TODO throw gun on death function

    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemy.stoppingDistance = attackRange;
        isIdle = true;
        hasReachedPlayer = false;
        isDead = false;
        enemy.speed = enemySpeed;
    }

    void Update()
    {
        if (isDead) //dead state
        {
            anim.enabled = false;
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
        

