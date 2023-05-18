using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    private bool isBobEnabled;
    private float baseAmplitude;
    public float baseFrequency;
    private float baseSpeed;
    private Vector3 startPos;
    private float stabAmount;

    public PlayerMovement player;
    public BobSettings settings;
    public Transform cam;
    public Transform camHolder;
    public Transform camPos;
    //public float frequency;
 
    // Start is called before the first frame update
    void Start()
    {
        InitialiseSettings();
        startPos = cam.localPosition;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isBobEnabled)
        {
            PlayMotion();
            ResetPosition();
            //cam.LookAt(FocusTarget());
        }
    }

    private void PlayMotion()
    {
        if (isBobEnabled && (player.isGrounded || player.isWallRunning) && player.isWASD)
        {
            cam.localPosition += cam.up * Mathf.Sin(Time.time * baseFrequency) * baseAmplitude;
            cam.localPosition += cam.right * Mathf.Cos(Time.time * baseFrequency / 2) * baseAmplitude / 2;
        }
    }

    private void ResetPosition()
    {
        if (cam.localPosition != startPos)
        {
            cam.localPosition = Vector3.Lerp(cam.localPosition, startPos, 1 * Time.deltaTime);
        }
    }

    private Vector3 FocusTarget()
    {
        Vector3 focusPos = new Vector3(transform.position.x, transform.position.y + camPos.localPosition.y, transform.position.z);
        focusPos += camHolder.forward * stabAmount; //stabAmount => distance of focal point for camera. Higher = More stable
        return focusPos;
    }

    private void InitialiseSettings()
    {
        isBobEnabled = settings.isBobEnabled;
        baseAmplitude = settings.baseAmplitude;
        baseFrequency = settings.baseFrequency;
        baseSpeed = settings.baseSpeed;
        stabAmount = settings.stabAmount;
    }
}
