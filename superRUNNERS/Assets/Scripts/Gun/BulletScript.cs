using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public Transform player;
    [SerializeField]
    private string playerName = "PlayerBody";
    private readonly string[] tagArr = { "Gun", "Player" };
    private Collider coll;

    [SerializeField]
    private float distFromPlayer;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider>();
        coll.isTrigger = true;
        player = GameObject.Find(playerName).GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dist = transform.position - player.position;
        if (dist.magnitude > distFromPlayer)
            Destroy(gameObject);
    }
       
    private void OnTriggerEnter(Collider other)
    {
        if (!tagArr.Contains(other.gameObject.tag))
            Destroy(gameObject);
    }
    
}
