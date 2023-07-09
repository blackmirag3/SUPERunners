using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSegmentControl : MonoBehaviour
{
    [SerializeField] private string PlayerTag;
    
    [SerializeField] private GameObject nextSegment;
    [SerializeField] private GameEvent updateExit;
    private GameObject currentSegment;
    //[SerializeField] private bool isLastRoom; //additional check on top of collider
    public DoorControl exitDoor;
    private ObjectiveManager objectives;
    private Collider exit;

    //public GameObject exitIndicator;

    private void Start()
    {
        objectives = GetComponent<ObjectiveManager>();
        exit = GetComponent<Collider>();
        exit.isTrigger = true;
        currentSegment = transform.gameObject;
        updateExit.CallEvent(this, nextSegment.transform.position);
    }

    public void EnableSegmentTrigger(Component sender, object data)
    {
        exit.enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (objectives != null)
        {
            if (!objectives.isComplete) return;
        }
        if (other.CompareTag(PlayerTag))
        {
            exit.enabled = false;
            if (exitDoor != null) exitDoor.Open();
            if (nextSegment != null) nextSegment.SetActive(true);
            currentSegment.SetActive(false);
            //TODO next room indicator?
            //exitIndicator.SetActive(false);
        }
    }
/*
    public void FinalRoomComplete(Component sender, object data)
    {
        //TODO
    }
    */
}
