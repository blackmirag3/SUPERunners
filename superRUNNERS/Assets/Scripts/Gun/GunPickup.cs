using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour, IHoldable
{

    public GunFire gun;
    public GunBreak gunBreak;
    public Rigidbody rb;
    public BoxCollider col;
    public Collider damageCol;
    public Rigidbody damageRb;

    public float throwForwardForce, throwUpForce;

    public bool equipped;
    private float despawnTime = 2f;

    public bool isGun { get; set; }

    void Start()
    {
        gun = GetComponent<GunFire>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();

        damageCol.enabled = false;
        damageRb.isKinematic = true;
        gunBreak.enabled = false;
        isGun = true;

        if (equipped)
        {
            gun.enabled = true;
            rb.isKinematic = true;
            col.isTrigger = true;
        }
        else if (!equipped)
        {
            gun.enabled = false;
            rb.isKinematic = false;
            col.isTrigger = false;
        }
    }

    public void Pickup(Transform hand)
    {
        equipped = true;

        // Disable forces acting on gun and BoxCollider a trigger
        rb.isKinematic = true;
        col.isTrigger = true;
        
        // Pickup by making gun a child of hand
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        // Enable gun shooting
        gun.enabled = true;
    }

    public void Throw(Vector3 point, Vector3 velocity)
    {
        equipped = false;

        transform.localPosition = Vector3.zero;
        transform.SetParent(null);
        
        // Enable damage hitboxes
        damageRb.isKinematic = false;
        damageCol.enabled = true;

        Vector3 dir = (point - transform.position).normalized;

        // Throw gun
        damageRb.velocity = velocity;
        damageRb.AddForce(dir * throwForwardForce, ForceMode.Impulse);
        damageRb.AddForce(transform.up * throwUpForce, ForceMode.Impulse);

        transform.localRotation = Random.rotation;
        float random = Random.Range(-1f, 1f);
        damageRb.AddTorque(new Vector3(random, random, random));

        gun.enabled = false;
        gunBreak.enabled = true;
       
        Destroy(gameObject, despawnTime);
    }

    public void SetItemInHand(Transform hand)
    {
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }
}
