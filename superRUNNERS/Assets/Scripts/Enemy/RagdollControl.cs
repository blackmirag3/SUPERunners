using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollControl : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody[] rigidbodies;
    public EnemyBehaviour enemyBehaviour;


    private void Awake()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        DisableRagdoll();
    }

    void Update()
    {
        if (enemyBehaviour.isDead)
            EnableRagdoll();
    }

    private void DisableRagdoll()
    {
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = true;
        }

    }

    private void EnableRagdoll()
    {
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = false;
        }

    }

}
