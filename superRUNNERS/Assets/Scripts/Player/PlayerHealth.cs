using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    // Placeholder for now

    [SerializeField] private float playerHealth;

    public void Damage(float damage)
    {
        Debug.Log($"Player Hit for {damage}");
    }

    private void Start()
    {
        
    }


    private void Update()
    {
        
    }
}
