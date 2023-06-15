using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Transform player;
    [SerializeField]
    private string playerName = "PlayerBody";
    private readonly string[] tagArr = { "Gun" , "Enemy"};

    [SerializeField]
    private float distFromPlayer;

    private void Start()
    {
        player = GameObject.Find(playerName).GetComponent<Transform>();   
    }

    
    private void Update()
    {
        Vector3 dist = transform.position - player.position;
        if (dist.magnitude > distFromPlayer)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!tagArr.Contains(other.tag))
        {
            DespawnBullet();
            // Debug.Log("Bullet Death");
        }
    }

    private void DespawnBullet()
    {
        Destroy(gameObject);
    }

}
