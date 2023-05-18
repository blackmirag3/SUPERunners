using System;
using UnityEngine;

public class WeaponMovement : MonoBehaviour {

    public WeaponMovementSettings settings;
    public PlayerCam playerCam;
    public PlayerMovement player;

    [Header("Sway")]
    //[SerializeField] private bool isSwayEnabled;
    [SerializeField] private float swaySmoothing;
    [SerializeField] private float tiltMultiplier;
    [SerializeField] private float rotMultiplier;
    [SerializeField] private float maxRot;
    [SerializeField] private bool tiltX;
    [SerializeField] private bool tiltY;
    [SerializeField] private bool tiltZ;
    private Quaternion swayAmount;
    private Quaternion tiltAmount;

    [Header("Bob")]
    //private bool isBobEnabled = true;
    private float bobFrequency = 16f;
    private float bobAmplitudeY = 0.0003f;
    private float bobAmplitudeX = 0.0001f;
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.localPosition;
        InitialiseSettings();
    }

    private void Update()
    {
        MoveSway();
        TiltSway();
        WeaponBob();
        CompositePositionRotation();
    }

    private void InitialiseSettings()
    {
        //isSwayEnabled = settings.isSwayEnabled;
        swaySmoothing = settings.swaySmoothing;
        tiltMultiplier = settings.tiltMultiplier;
        rotMultiplier = settings.rotMultiplier;
        maxRot = settings.maxRot;
        tiltX = settings.tiltX;
        tiltY = settings.tiltY;
        tiltZ = settings.tiltZ;
    }

    private void WeaponBob()
    {
        if ((player.isGrounded || player.isWallRunning) && player.isWASD)
        {
            transform.localPosition += -transform.up * Mathf.Sin(Time.time * bobFrequency) * bobAmplitudeY;
            transform.localPosition += transform.right * Mathf.Cos(Time.time * bobFrequency / 2) * bobAmplitudeX;
        }

        if (transform.localPosition != startPos) //Reset pos
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPos, 3f * Time.deltaTime);
        }
    }

    private void MoveSway()
    {
        Quaternion rotationX = Quaternion.AngleAxis(playerCam.mouseY * tiltMultiplier, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(-playerCam.mouseX * tiltMultiplier, Vector3.up);
        swayAmount = rotationX * rotationY;
    }

    private void TiltSway()
    {
        float rotationY = Mathf.Clamp(playerCam.mouseX * rotMultiplier, -maxRot, maxRot);
        float rotationX = Mathf.Clamp(playerCam.mouseY * rotMultiplier, -maxRot, maxRot);
        tiltAmount = Quaternion.Euler(new Vector3(
            tiltX ? -rotationX : 0,
            tiltY ? rotationY : 0,
            tiltZ ? rotationY : 0
            ));
    }

    void CompositePositionRotation()
    {
        //transform.localPosition = Vector3.Lerp(transform.localPosition, bobPosition, Time.deltaTime * swaySmoothing); //Reset pos
        transform.localRotation = Quaternion.Slerp(transform.localRotation, swayAmount * tiltAmount, Time.deltaTime * swaySmoothing);
    }
}
