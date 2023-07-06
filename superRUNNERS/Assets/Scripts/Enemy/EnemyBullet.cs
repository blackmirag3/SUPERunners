using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private readonly string[] tagArr = { "Gun" , "Enemy" , "PlayerTriggers" };

    [SerializeField]
    private float despawnTime;

    private void Start()
    {
        Destroy(gameObject, despawnTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!tagArr.Contains(other.tag))
        {
            Destroy(gameObject);
            // Debug.Log("Bullet Death");
        }
    }

}
