using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletTrigger : MonoBehaviour
{
    [SerializeField]
    private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Destroy(transform.parent.gameObject);
            //Debug.Log("Player hit detected");
        }
            
    }
}
