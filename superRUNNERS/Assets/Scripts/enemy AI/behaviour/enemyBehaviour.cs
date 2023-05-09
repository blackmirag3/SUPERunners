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
    Vector3 deltaDistance;                                                                                                                                                                                      

    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemy.stoppingDistance = 1;
        idleState = true;
        //stoppingDistance = 1;
        //aggroDistance = 2;
    }
    /*
    void Update()
    {
        deltaDistance = enemy.position - player.position;
        if (deltaDistance < 1) //enemy aggro'd
        {
            idleState = false;
            anim.SetBool(isIdle, idleState);
        }

        if (!idleState)
        {
            enemy.SetDestination(player.position);
            if (deltaDistance < 1)
            {
                enemy.SetDestination();
                anim.SetBool(hasReachedPlayer, true);
                //TODO attack
            }
            else
            {
                anim.SetBool(hasReachedPlayer, false);
            }
        }
    }*/
}
