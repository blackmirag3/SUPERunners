using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public Transform rightDoor;
    public Transform leftDoor;

    [SerializeField] private float speed;
    [SerializeField] private float doorSlideDist;

    private Vector3 rightClosePos, leftClosePos, rightOpenPos, leftOpenPos;

    private bool isOpen;

    private void Start()
    {
        rightClosePos = rightDoor.localPosition;
        leftClosePos = leftDoor.localPosition;

        rightOpenPos = rightClosePos + (doorSlideDist * Vector3.forward);
        leftOpenPos = leftClosePos + (doorSlideDist * Vector3.back);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }
    }

    public void Open()
    {
        if (!isOpen)
        {
            isOpen = true;
            StartCoroutine(OpenDoors());
        }
        else
        {
            Debug.Log("Cannot open");
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            isOpen = false;
            StartCoroutine(CloseDoors());
            
        }
        else
        {
            Debug.Log("Cannot close");
        }
        enabled = false;
    }

    private IEnumerator OpenDoors()
    {
        float time = 0;
        while (time < 1)
        {
            rightDoor.transform.localPosition = Vector3.Lerp(rightClosePos, rightOpenPos, time);
            leftDoor.transform.localPosition = Vector3.Lerp(leftClosePos, leftOpenPos, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }

    private IEnumerator CloseDoors()
    {
        float time = 0;
        while (time < 1)
        {
            rightDoor.transform.localPosition = Vector3.Lerp(rightOpenPos, rightClosePos, time);
            leftDoor.transform.localPosition = Vector3.Lerp(leftOpenPos, leftClosePos, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            Close();
    }
}
