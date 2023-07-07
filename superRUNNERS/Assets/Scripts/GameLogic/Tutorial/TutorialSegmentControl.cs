using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSegmentControl : MonoBehaviour
{
    public Collider col;
    [SerializeField] private string PlayerTag;
    [SerializeField] private GameObject nextSegment;
    [SerializeField] private GameObject currentSegment;
    //[SerializeField] private bool isLastRoom; //additional check on top of collider
    public DoorControl door;

    //public GameObject exitIndicator;

    private void Start()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    public void EnableSegmentTrigger(Component sender, object data)
    {
        col.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            col.enabled = false;
            if (door != null) door.Open();
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
