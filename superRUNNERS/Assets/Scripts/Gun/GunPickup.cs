using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.CompilerServices; 
[assembly: InternalsVisibleTo( "PlayMode" )]
[assembly: InternalsVisibleTo( "EditMode" )]

public class GunPickup : MonoBehaviour, IHoldable
{

    public GunFire gun;
    public GunBreak gunBreak;
    public Rigidbody rb;
    public BoxCollider col;
    public Collider damageCol;
    public Rigidbody damageRb;

    public float throwForwardForce, throwUpForce;

    internal float despawnTime = 2f;
    public bool isEquipped {get; set;}
    public bool isGun { get; set; }

    //for unity test
    internal bool isGunEnabled;
    internal bool isGunBreakEnabled;

    void Start()
    {
        gun = GetComponent<GunFire>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();

        damageCol.enabled = false;
        damageRb.isKinematic = true;
        gunBreak.enabled = false;
        isGun = true;

        if (isEquipped)
        {
            gun.enabled = true;
            rb.isKinematic = true;
            col.isTrigger = true;
        }
        else if (!isEquipped)
        {
            gun.enabled = false;
            rb.isKinematic = false;
            col.isTrigger = false;
        }
    }

    public void Pickup(Transform hand)
    {
        isEquipped = true;

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

    public void Throw(Vector3 point)
    {
        isEquipped = false;

        transform.localPosition = Vector3.zero;
        transform.SetParent(null);
        
        // Enable damage hitboxes
        damageRb.isKinematic = false;
        damageCol.enabled = true;

        Vector3 dir = (point - transform.position).normalized;

        // Throw gun
        damageRb.AddForce(dir * throwForwardForce, ForceMode.Impulse);
        damageRb.AddForce(transform.up * throwUpForce, ForceMode.Impulse);

        transform.localRotation = Random.rotation;
        float random = Random.Range(-1f, 1f);
        damageRb.AddTorque(new Vector3(random, random, random));

        isGunEnabled = false;
        isGunBreakEnabled = true;
        gun.enabled = isGunEnabled;
        gunBreak.enabled = isGunBreakEnabled;
       
        Destroy(gameObject, despawnTime);
        this.enabled = false;
    }

    public void SetItemInHand(Transform hand)
    {
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }
}
