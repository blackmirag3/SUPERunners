using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    private bool enable;
    private float amplitude;
    private float frequency;
    private float toggleSpeed;
    private Vector3 startPos;
    private float stabAmount;

    public PlayerMovement player;
    public BobSettings settings;
    public Transform cam;
    public Transform cameraHolder;

    // Start is called before the first frame update
    void Start()
    {
        InitialiseSettings();
        startPos = cam.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (enable)
        {
            CheckMotion();
            ResetPosition();
            cam.LookAt(FocusTarget());
        }
    }

    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude * 2;
        return pos;
    }

    private void CheckMotion()
    {
        float speed = player.currVelocity.magnitude;
        if (speed > toggleSpeed && player.isGrounded)
        {
            cam.localPosition += FootStepMotion();
        }
    }

    private void ResetPosition()
    {
        if (cam.localPosition == startPos) return;
        cam.localPosition = Vector3.Lerp(cam.localPosition, startPos, 1 * Time.deltaTime);
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + cameraHolder.localPosition.y, transform.position.z);
        pos += cameraHolder.forward * stabAmount; //stabAmount => distance of focal point for camera. Higher = More stable
        return pos;
    }

    private void InitialiseSettings()
    {
        enable = settings.enable;
        amplitude = settings.amplitude;
        frequency = settings.frequency;
        toggleSpeed = settings.toggleSpeed;
        stabAmount = settings.stabAmount;
    }
}
