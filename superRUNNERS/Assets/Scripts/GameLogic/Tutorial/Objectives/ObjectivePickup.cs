using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivePickup : MonoBehaviour, IObjective
{
    [SerializeField] private GameObject targetItem;
    private IHoldable holdable;
    public bool isComplete { get; set; }
    public void CheckProgress()
    {
        if (holdable.isEquipped) isComplete = true;
    }
    private void Start()
    {
        holdable = targetItem.GetComponent<IHoldable>();
    }
    private void Update()
    {
        if (holdable == null) holdable = targetItem.GetComponent<IHoldable>();
        else if (!isComplete) CheckProgress();
    }
}
