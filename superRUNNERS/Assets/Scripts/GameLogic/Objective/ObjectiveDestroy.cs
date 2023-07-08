using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveDestroy : MonoBehaviour, IObjective
{
    public GameEvent objective;
    [SerializeField] private GameObject toDestroy;
     public void ObjectiveComplete()
    {
        objective.CallEvent(this, null);
    }
    private void Update()
    {
        if (toDestroy == null) ObjectiveComplete();
    }
}
