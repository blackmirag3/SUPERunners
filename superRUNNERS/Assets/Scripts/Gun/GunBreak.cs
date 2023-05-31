using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBreak : MonoBehaviour
{
    public Collider col;
    private bool hasHit;
    public GameObject gunShattered;

    private void OnEnable()
    {
        hasHit = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasHit)
        {
            col.enabled = false;
            hasHit = true;
            Instantiate(gunShattered, transform.position, transform.rotation, transform.parent);
            Destroy(gameObject);
        }   
    }
}
