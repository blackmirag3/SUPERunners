using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{

    public Camera playerCam;
    public Transform player;

    public float pickupRange;
    public KeyCode pickupKey = KeyCode.Mouse1;
    public KeyCode throwKey = KeyCode.Mouse0;
    public static bool handEmpty = true;

    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (handEmpty && Input.GetKeyDown(pickupKey))
        {
            Ray camPoint = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
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
        else if (!handEmpty)
        {
            IHoldable throwable = GetComponentInChildren<IHoldable>();
            bool mustThrow = throwable.MustThrow();
            if ((mustThrow && Input.GetKeyDown(throwKey)) || Input.GetKeyDown(pickupKey))
            {
                Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

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
    }
}
