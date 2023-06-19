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

    private const string punchTrigger = "Punch";
    private const string punchAnimState = "ArmReturn";
    [SerializeField] private float returnTransitionTime;
    

    [SerializeField] private Animator handAnim;

    public float pickupRange;
    public KeyCode pickupKey = KeyCode.Mouse1;
    public KeyCode attackKey = KeyCode.Mouse0;
    public static bool handEmpty = true;

    private bool itemIsGun;
    private bool isPaused;

    [SerializeField]
    private float meleeActionDur, throwActionDur, pickActionDur;

    public GameEvent onPlayerAction;
    [SerializeField] WallRunning wallRun;

    private void Awake()
    {
        handAnim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        CheckHandOnStart();
        isPaused = false;
        handHitbox = GetComponentInChildren<Collider>();
        handHitbox.enabled = false;
        handHitbox.isTrigger = true;

        itemIsGun = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isPaused)
        {
            if (handEmpty && Input.GetKeyDown(pickupKey))
            {
                PickupItem();
            }
            else if (handEmpty && Input.GetKeyDown(attackKey) && canPunch)
            {
                MeleeAttack();
            }
            else if (!handEmpty && (Input.GetKeyDown(pickupKey) || (!itemIsGun && Input.GetKeyDown(attackKey))))
            {
                ThrowItem();
            }
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
                onPlayerAction.CallEvent(this, pickActionDur);
                
                handAnim.CrossFadeInFixedTime(punchAnimState, returnTransitionTime);

                // Hand children index - 0 right, 1 left
                if (wallRun.defaultHandActive)
                {
                    grab.Pickup(transform.GetChild(0));
                }
                else
                {
                    grab.Pickup(transform.GetChild(1));
                }
                
                itemIsGun = grab.isGun;
                Debug.Log(itemIsGun);
            }
        }
    }

    private void ThrowItem()
    {
        IHoldable throwable = GetComponentInChildren<IHoldable>();
       
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
        onPlayerAction.CallEvent(this, throwActionDur);

        Vector3 throwDir = targetPoint - transform.position;
        Vector3 playerVelocity = player.GetComponent<Rigidbody>().velocity;
        Vector3 throwVelocity = new Vector3(playerVelocity.x, 0, playerVelocity.z);

        throwable.Throw(throwDir, throwVelocity);
        handEmpty = true;


    }

    private void MeleeAttack()
    {
        onPlayerAction.CallEvent(this, meleeActionDur);
        
        canPunch = false;       
        handHitbox.enabled = true;

        // Animate melee punch
        handAnim.SetTrigger(punchTrigger);
        
        Invoke(nameof(ResetMelee), punchCD);

    }
    
    private void ResetMelee()
    {
        handHitbox.enabled = false;
        canPunch = true;
    }
    
    private void CheckHandOnStart()
    {
        IHoldable item = GetComponentInChildren<IHoldable>();
        if (item == null)
        {
            handEmpty = true;
        }
        else
        {
            handEmpty = false;
        }
    }

    public void PauseCalled(Component sender, object data)
    {
        if (data is bool)
        {
            isPaused = (bool)data;
            return;
        }
        Debug.Log($"Unwanted event call from {sender}");
    }
}
