using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public Transform player;
    [SerializeField]
    private string playerName = "PlayerBody";
    private readonly string[] tagArr = { "Gun" };
    private Collider playerCol;

    [SerializeField]
    private string enemyTag = "Enemy";
    [SerializeField]
    private float distFromPlayer;

    // Start is called before the first frame update
    void Start()
    {
        // playerCol = GameObject.Find(playerName).GetComponent<Collider>();
        player = GameObject.Find(playerName).GetComponent<Transform>();
        // Physics.IgnoreCollision(playerCol, GetComponent<Collider>());
        // Invoke(nameof(EnablePlayerColl), 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dist = transform.position - player.position;
        if (dist.magnitude > distFromPlayer)
            DespawnBullet();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!tagArr.Contains(collision.gameObject.tag))
        {
            if (collision.gameObject.CompareTag(enemyTag))
                Debug.Log("Enemy hit");

            DespawnBullet();
        }
        
    }


    /*
    private void EnablePlayerColl()
    {
        Physics.IgnoreCollision(playerCol, GetComponent<Collider>(), false);
    }
    */

    private void DespawnBullet()
    {
        Destroy(gameObject);
    }
    

}
