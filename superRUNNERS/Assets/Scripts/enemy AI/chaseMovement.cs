using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnemy : MonoBehaviour
{
    public float speed;
    public Transform target;
    public float minimumDistance;

    private void Update()
    {
        //out of range, run towards player
        if (Vector2.Distance(transform.position, target.position) > minimumDistance) 
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}
