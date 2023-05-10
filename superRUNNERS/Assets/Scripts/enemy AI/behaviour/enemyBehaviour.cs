using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyBehaviour : MonoBehaviour
{
    private NavMeshAgent enemy;
    public Transform player;
    public Animator anim;   
    bool idleState;
    bool reachedPlayerState;
    bool isAggro = true; //Set to false after implementing aggro mechanics
    //Vector3 deltaDistance;

    //TODO
    //bool checkAggro(){}

    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemy.stoppingDistance = 3f;
        idleState = true;
        reachedPlayerState = false;
    }
    void Update()
    {
        if (idleState)
        {
            //isAgro = checkAggro();
            idleState = false;
            anim.SetBool("isIdle", idleState); //set animator state
        }

        if (!idleState)
        {
            enemy.SetDestination(player.position); //chase player
            /*if (deltaDistance < 1)
            {
                enemy.SetDestination();
                anim.SetBool("hasReachedPlayer", true); //set animator state
                //TODO attack
            }
            else
            {
                anim.SetBool("hasReachedPlayer", false); //set animator state
            }*/
        }
    }
}
