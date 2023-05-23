using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour, IHoldable
{

    public GunFire gun;
    public Rigidbody rb;
    public BoxCollider col;
    public Collider damageCol;
    public Rigidbody damageRb;

    public float throwForwardForce, throwUpForce;

    public bool equipped;
    public bool outOfAmmo = false;
    private float despawnTime = 2f;
    private bool enableDespawn = false;

    void Start()
    {
        gun = GetComponent<GunFire>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();

        outOfAmmo = false;
        enableDespawn = false;
        damageCol.enabled = false;
        damageRb.isKinematic = true;

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

    // Update is called once per frame
    private void Update()
    {
        if (equipped && gun.ammoLeft == 0)
        {
            outOfAmmo = true;
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

    public void Throw(Vector3 dir, Vector3 velocity)
    {
        equipped = false;

        transform.SetParent(null);
        
        // Enable damage hitboxes
        damageRb.isKinematic = false;
        damageCol.enabled = true;

        // Throw gun
        damageRb.velocity = velocity;
        damageRb.AddForce(dir.normalized * throwForwardForce, ForceMode.Impulse);
        damageRb.AddForce(transform.up * throwUpForce, ForceMode.Impulse);

        float random = Random.Range(-1f, 1f);
        damageRb.AddTorque(new Vector3(random, random, random));

        gun.enabled = false;

        enableDespawn = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enableDespawn)
        {
            StartCoroutine(DespawnGun());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enableDespawn)
        {
            StartCoroutine(DespawnGun());
        }
    }

    IEnumerator DespawnGun()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }
}
