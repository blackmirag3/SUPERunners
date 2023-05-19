using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{

    public Camera playerCam;
    public Transform player;
    public Collider handHitbox;

    public float meleeReach;
    private bool canPunch = true;
    public float punchCD;

    public float pickupRange;
    public KeyCode pickupKey = KeyCode.Mouse1;
    public KeyCode throwKey = KeyCode.Mouse0;
    public static bool handEmpty = true;

    void Start()
    {
        handHitbox = GetComponentInChildren<Collider>();
        handHitbox.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (handEmpty && Input.GetKeyDown(pickupKey))
        {
            PickupItem();
        }
        else if (handEmpty && Input.GetKeyDown(throwKey) && canPunch)
        {
            MeleeAttack();
        }
        else if (!handEmpty)
        {
            ThrowItem();
        }
    }

    private Ray GetCamRay()
    {
        return playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    }

    private void PickupItem()
    {
        Ray camPoint = GetCamRay();
        RaycastHit hit;

        if (Physics.Raycast(camPoint, out hit, pickupRange))
        {
            IHoldable grab = hit.collider.GetComponent<IHoldable>();
            if (grab != null)
            {
                handEmpty = false;
                grab.Pickup(transform);
            }
        }
    }

    private void ThrowItem()
    {
        IHoldable throwable = GetComponentInChildren<IHoldable>();
        bool mustThrow = throwable.MustThrow();
        if ((mustThrow && Input.GetKeyDown(throwKey)) || Input.GetKeyDown(pickupKey))
        {
            Ray ray = GetCamRay();

            Vector3 targetPoint;
            // Check if ray hits
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(75);
            }

            Vector3 throwDir = targetPoint - transform.position;
            Vector3 playerVelocity = player.GetComponent<Rigidbody>().velocity;
            Vector3 throwVelocity = new Vector3(playerVelocity.x, 0, playerVelocity.z);

            throwable.Throw(throwDir, throwVelocity);
            handEmpty = true;
        }

    }

    private void MeleeAttack()
    {
        GetCamRay();
        canPunch = false;
        // Animate melee punch
        /*
        Animator handAnim = GetComponent<Animator>();
        handAnim.SetTrigger("Punch");
        Invoke(nameof(ResetMelee), punchCD);
        */
    }

    private void ResetMelee()
    {
        canPunch = true;
    }

}
