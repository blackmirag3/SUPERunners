using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFracture : MonoBehaviour
{
    public float minExplosionForce;
    public float maxExplosionForce;
    public Rigidbody[] rbs;

    private void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rbs)
        {
            float force = Random.Range(minExplosionForce, maxExplosionForce);
            rb.AddExplosionForce(force, transform.position, 10f);
        }
        
    }

}
