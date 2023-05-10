using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public Transform player;
    string playerName = "PlayerBody";
    string pTag = "Player";

    [SerializeField]
    private float distFromPlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find(playerName).GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dist = transform.position - player.position;
        if (dist.magnitude > distFromPlayer)
            Destroy(gameObject);
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != pTag)
            Destroy(gameObject);
    }
    
}
