using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveCollect : MonoBehaviour, IObjective
{
    [SerializeField] private GameObject targetItem;
    private ICollectible collectible;
    public bool isComplete { get; set; }
    public void CheckProgress()
    {
        if (collectible.isEquipped) isComplete = true;
    }
    private void Start()
    {
        collectible = targetItem.GetComponent<ICollectible>();
    }
    private void Update()
    {
        if (collectible != null)
        {
            if (!isComplete) CheckProgress();
        }
    }
}
