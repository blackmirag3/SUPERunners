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
    private bool isDead;
    public float attackRangecurrentlydoesnothing;
    public EnemyGun enemyGun;

    //TODO check dead function
    //bool checkDead?

    void enemyChase()
    {
        enemy.SetDestination(player.position); //chase player
        hasReachedPlayer = (enemy.remainingDistance <= enemy.stoppingDistance);
        anim.SetBool("hasReachedPlayer", hasReachedPlayer);
    }

    bool hasLOS()
    {
        bool hasLOS = false;
        NavMeshHit hit;

        if (!enemy.Raycast(player.position, out hit)) //if no obstacle between enemy LOS and player
        {
            hasLOS = true;
        }
        Debug.Log("enemy has LOS: " + hasLOS);
        return hasLOS;
    }

    void enemyShoot() //called by animation event
    {
        if (hasLOS())
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
        enemy.stoppingDistance = 3f;
        isIdle = true;
        hasReachedPlayer = false;
        isDead = false;
    }
    void Update()
    {
        if (isDead) //dead state
        {
            anim.SetBool("isDead", isDead);
            Debug.Log("enemy is dead:" + isDead);
        }

        else if (isIdle) //idle state
        {
            //TODO check aggro function (idle -> aggro)
            isIdle = false; //assumed aggro
            anim.SetBool("isIdle", isIdle);
        }

        else //aggro state
        {
            enemyChase();
            //TODO different chase states
            if (hasLOS())
            {
            }

        }

        Debug.Log("enemy is idle: " + isIdle);
        Debug.Log("enemy has reached player: " + hasReachedPlayer);
    }
}
        

