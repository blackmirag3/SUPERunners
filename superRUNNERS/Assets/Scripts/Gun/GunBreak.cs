using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBreak : MonoBehaviour
{
    public Collider col;
    private bool hasHit;

    private void OnEnable()
    {
        hasHit = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasHit)
        {
            col.enabled = false;
            GetComponentInParent<Rigidbody>().isKinematic = false;
            GetComponentInParent<Collider>().isTrigger = false;
            hasHit = true;
        }   
    }
}
