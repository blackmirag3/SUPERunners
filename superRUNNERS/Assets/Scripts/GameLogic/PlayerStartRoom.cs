using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartRoom : MonoBehaviour
{
    public Collider col;
    [SerializeField]
    private string playerTag;

    public GameEvent nextLevel;

    public DoorControl entryDoor;
    public DoorControl exitDoor;
    public GameObject exitIndicator;

    private void Start()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    public void LevelComplete(Component sender, object data)
    {
        col.enabled = true;
        if (exitDoor.enabled)
        {
            exitDoor.Open();
            exitIndicator.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            nextLevel.CallEvent(this, null);
            col.enabled = false;
            entryDoor.Open();
            exitDoor.Close();
            exitIndicator.SetActive(false);
        }
    }
}
