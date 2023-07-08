using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivePickup : MonoBehaviour, IObjective
{
    [SerializeField] private IHoldable holdable;
    public GameEvent objective;
    public void ObjectiveComplete()
    {
        objective.CallEvent(this, null);
    }
    private void Update()
    {
        if (holdable.isEquipped)
        {
            ObjectiveComplete();
        }
    }
}
