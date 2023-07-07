using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviourOld : MonoBehaviour
{
    private NavMeshAgent enemy;
    public Transform Player;
    public Animator anim;   
    bool isIdle;
    bool hasReachedPlayer;
    bool isDead;
    public bool isDebugMode; //true to enable manual aggro via raycast from debug cam
    public Camera debugCam;
    //bool isAggro = true;

    //TODO check aggro function
    //bool checkAggro(){}

    //TODO check dead function
    //bool checkDead?

    //TODO helper functions for check idle, has reached Player etc

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
        if (isDead)
        {
            anim.SetBool("isDead", isDead);
//Debug.Log("enemy is dead:" + isDead);
        }

        if (isDebugMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = debugCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                Physics.Raycast(ray, out hit); //if raycast hit
                {
                    //move agent
                    enemy.SetDestination(hit.point);
                    //isAgro = true;
                    isIdle = false;
                    anim.SetBool("isIdle", isIdle);
                }
            }
            hasReachedPlayer = (enemy.remainingDistance <= enemy.stoppingDistance);
            anim.SetBool("hasReachedPlayer", hasReachedPlayer);
        }

        else
        {
            if (isIdle)
            {
                //isAgro = checkAggro();
                isIdle = false;
                anim.SetBool("isIdle", isIdle);
//Debug.Log("enemy is idle:" + isIdle);
            }

            else
            {
                enemy.SetDestination(Player.position); //chase Player
                hasReachedPlayer = (enemy.remainingDistance <= enemy.stoppingDistance);
                anim.SetBool("hasReachedPlayer", hasReachedPlayer);
//Debug.Log("enemy has reached Player:" + hasReachedPlayer);
            }
        }
        Debug.Log("enemy is idle:" + isIdle);
        Debug.Log("enemy has reached Player:" + hasReachedPlayer);
    }
}
