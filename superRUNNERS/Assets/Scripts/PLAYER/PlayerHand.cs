using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHand : MonoBehaviour
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

    private PlayerInput input = null;
    private InputAction fireInput = null;
    private InputAction pickupInput = null;

    private void Awake()
    {
        input = InputManager.instance.PlayerInput;
        fireInput = input.actions["Fire"];
        pickupInput = input.actions["Item Interact"];

        fireInput.performed += PunchCall;
        pickupInput.performed += PickupCall;

        handAnim = GetComponentInChildren<Animator>();
    }

    private void OnDisable()
    {
        fireInput.performed -= PunchCall;
        pickupInput.performed -= PickupCall;
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

    private void PunchCall(InputAction.CallbackContext ctx)
    {
        if (isPaused)
        {
            return;
        }

        if (handEmpty && canPunch)
        {
            MeleeAttack();
        }
        else if (!handEmpty && !itemIsGun)
        {
            ThrowItem();
        }
    }

    private void PickupCall(InputAction.CallbackContext ctx)
    {
        if (isPaused)
        {
            return;
        }

        if (handEmpty)
        {
            PickupItem();
        }
        else
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
                StartCoroutine(SetEmptyHandBool(false));
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
            else
            {
                ICollectible collectible = hit.collider.GetComponent<ICollectible>();
                if (collectible != null)
                {
                    collectible.OnPickup();
                }
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

        throwable.Throw(targetPoint);
        StartCoroutine(SetEmptyHandBool(true));
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

    private IEnumerator SetEmptyHandBool(bool state)
    {
        yield return null;
        handEmpty = state;
    }
}
