using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private readonly string[] tagArr = { "Gun" };
    public float damage = 0;


    [SerializeField]
    private string enemyTag = "Enemy";
    [SerializeField]
    private float despawnTime;

    private void Start()
    {
        Destroy(gameObject, despawnTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!tagArr.Contains(collision.gameObject.tag))
        {
            if (collision.gameObject.CompareTag(enemyTag))
            {
                // Debug.Log("Enemy hit");
            }

            Destroy(gameObject);
        }
        
    }
    

}
