using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCameraLook : MonoBehaviour
{
    [SerializeField] Transform playerCam;
    [SerializeField] GameObject playerInput;
    [SerializeField] Transform target;
    private bool isLooking;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StopAndLook());
    }

    void Update()
    {
        if (isLooking)
        {
            playerCam.LookAt(target);
        }
    }

    private IEnumerator StopAndLook()
    {
        playerInput.SetActive(false);
        if (playerCam != null && target != null) isLooking = true;
        yield return new WaitForSecondsRealtime(2);
        isLooking = false;
        playerInput.SetActive(true);
        yield return null;
    }
}
