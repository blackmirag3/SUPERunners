using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.CompilerServices; 
[assembly: InternalsVisibleTo( "PlayMode" )]
[assembly: InternalsVisibleTo( "EditMode" )]

public class WatchCollectible : MonoBehaviour, ICollectible
{
    [SerializeField] Transform handSlot;
    [SerializeField] TimeControl timeController;
    public bool isEquipped {get; set;}
    public Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
    }

    public void OnPickup()
    {
        isEquipped = true;
        if (timeController != null) timeController.enabled = true;
        // Pickup by making gun a child of hand
        transform.SetParent(handSlot);
        SetItemInHand();
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        col.enabled = false;
        //transform.localScale = Vector3.one;
    }
    private void SetItemInHand()
    {
        transform.SetParent(handSlot);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }
}
