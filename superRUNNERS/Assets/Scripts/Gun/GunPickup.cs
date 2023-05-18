using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{

    public Transform player, hand;
    public Camera playerCam;
    public GunFire gun;
    public Rigidbody rb;
    public BoxCollider col;
    
    public float pickupRange;
    public float throwForwardForce, throwUpForce;

    public bool equipped;
    public static bool handFull;

    [SerializeField] private string handName = "Hand";
    [SerializeField] private string playerName = "PlayerBody";
    [SerializeField] private string camName = "PlayerCam";

    [Header("Pickup/Throw Key")]
    public KeyCode pickupKey = KeyCode.Mouse2;
    public KeyCode throwKey = KeyCode.Mouse1;

    void Start()
    {
        gun = GetComponent<GunFire>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        hand = GameObject.Find(handName).GetComponent<Transform>();
        player = GameObject.Find(playerName).GetComponent<Transform>();
        playerCam = GameObject.Find(camName).GetComponent<Camera>();
        
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
        Vector3 distToPlayer = player.position - transform.position;
        if (!handFull && !equipped && distToPlayer.magnitude <= pickupRange && Input.GetKeyDown(pickupKey))
        {
            Ray camPoint = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); ;
            RaycastHit hit;
            if (Physics.Raycast(camPoint, out hit))
            {
                //Debug.Log("Gun pick up ray hit");

                if (hit.transform.position == transform.position)
                {
                    // Debug.Log("True");
                    Pickup();
                }
            }
        }

        if (equipped)
        {
            if ((gun.ammoLeft == 0 && Input.GetKeyDown(KeyCode.Mouse0)) || Input.GetKeyDown(throwKey))
            {
                ThrowGun();
            }
        }
    }

    private void Pickup()
    {
        equipped = true;
        handFull = true;

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

    private void ThrowGun()
    {
        equipped = false;
        handFull = false;

        transform.SetParent(null);
        
        // Enable forces on gun
        rb.isKinematic = false;
        col.isTrigger = false;

        // Throw gun
        rb.velocity = player.GetComponent<Rigidbody>().velocity;
        rb.AddForce(playerCam.GetComponent<Transform>().forward * throwForwardForce, ForceMode.Impulse);
        rb.AddForce(playerCam.GetComponent<Transform>().up * throwUpForce, ForceMode.Impulse);

        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random));

        gun.enabled = false;
    }
}
