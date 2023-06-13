using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartRoom : MonoBehaviour
{
    public Collider col;
    [SerializeField]
    private string playerTag;

    public GameEvent onLevelComplete;

    private void Start()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;
        col.enabled = false;
    }

    public void LevelComplete(Component sender, object data)
    {
        col.enabled = true;
    }

    private void OpenDoor()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            onLevelComplete.CallEvent(this, null);
            col.enabled = false;
        }
    }
}
