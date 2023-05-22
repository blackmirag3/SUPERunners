using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour, IHoldable
{

    public GunFire gun;
    public Rigidbody rb;
    public BoxCollider col;
    
    public float throwForwardForce, throwUpForce;

    public bool equipped;
    public bool outOfAmmo = false;
    private float despawnTime = 2f;

    void Start()
    {
        gun = GetComponent<GunFire>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();

        outOfAmmo = false;

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
        // Player in range
        

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
        
        // Enable forces on gun
        rb.isKinematic = false;
        col.isTrigger = false;

        // Throw gun
        rb.velocity = velocity;
        rb.AddForce(dir.normalized * throwForwardForce, ForceMode.Impulse);
        rb.AddForce(transform.up * throwUpForce, ForceMode.Impulse);

        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random));

        gun.enabled = false;
    }

    public bool MustThrow()
    {
        return outOfAmmo;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
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
