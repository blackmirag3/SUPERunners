using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentObjectives : ObjectiveManager
{
    /*
    [SerializeField] private ObjectivePickup toPickup;
    [SerializeField] private ObjectiveThrow toThrow;
    [SerializeField] private ObjectiveDestroy toDestroy;
    */
    private IObjective[] objectives;

    void Start()
    {
        /*
        objectives[0] = toPickup;
        objectives[1] = toDestroy;
        objectives[2] = toThrow;
        */
        objectives = GetComponents<IObjective>();
    }

    void Update()
    {
        if (!isComplete)
        {
        StartCoroutine(CheckProgress());
        }
    }
    IEnumerator CheckProgress()
    {
        isComplete = UpdateProgress(objectives);
        yield return new WaitForSeconds(1);
    }
}
