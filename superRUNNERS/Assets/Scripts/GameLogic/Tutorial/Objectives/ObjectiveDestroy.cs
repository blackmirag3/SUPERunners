using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectiveDestroy : MonoBehaviour, IObjective
{
    public GameObject targetItem;
    public bool isComplete { get; set; }
    public void CheckProgress()
    {
        if (targetItem == null) isComplete = true;
    }
    private void Update()
    {
        if (!isComplete) CheckProgress();
    }
}
