using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHoldable
{
    void Pickup(Transform hand);

    void Throw(Vector3 dir);

    bool isGun { get; set; }

    void SetItemInHand(Transform hand);

    bool isEquipped {get; set;}
}
