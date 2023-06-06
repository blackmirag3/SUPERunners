using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IHoldable
{
    public float throwForce, throwUpForce;

    public Rigidbody rb;
    public Collider col;

    public GameObject itemThrown;

    public bool isGun { get; set; }

    private void Start()
    {
        rb.isKinematic = false;
        col.isTrigger = false;

        isGun = false;
    }
    
    public void Pickup(Transform hand)
    {
        rb.isKinematic = true;
        col.isTrigger = true;

        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public void Throw(Vector3 dir, Vector3 velocity)
    {
        GameObject newItem = Instantiate(itemThrown, transform.position, Random.rotation);
        Rigidbody thrownItemRb = newItem.GetComponent<Rigidbody>();
        thrownItemRb.velocity = velocity;
        thrownItemRb.AddForce(dir.normalized * throwForce, ForceMode.Impulse);
        thrownItemRb.AddForce(transform.up * throwUpForce, ForceMode.Impulse);

        float random = Random.Range(-1f, 1f);
        thrownItemRb.AddTorque(new Vector3(random, random, random));

        Destroy(gameObject);
    }
}
