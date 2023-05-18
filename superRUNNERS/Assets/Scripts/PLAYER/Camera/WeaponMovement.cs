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
    private float bob;
    private float bobFrequency;

    private void Start()
    {
        //startPos = transform.localPosition;
        InitialiseSettings();
    }

    private void Update()
    {
        MoveSway();
        TiltSway();
        //WeaponShift();
        //BobOffset();
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
    /*
    private void WeaponBob()
    {
        transform.localPosition += cam.up * Mathf.Sin(Time.time * baseFrequency) * bobAmplitude;
        transform.localPosition += cam.right * Mathf.Cos(Time.time * baseFrequency / 2) * bobAmplitude / 2;

        //if (transform.localPosition != startPos)
    }

    private void ResetPosition()
    {
        if (localPosition != startPos)
        {
            cam.localPosition = Vector3.Lerp(cam.localPosition, startPos, 1 * Time.deltaTime);
        }
    }*/

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
       // transform.localPosition = Vector3.Lerp(transform.localPosition, bobPosition, Time.deltaTime * swaySmoothing);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, swayAmount * tiltAmount, Time.deltaTime * swaySmoothing);
    }

    /*
    void BobOffset()
    {
        speedCurve += Time.deltaTime * (player.isGrounded ? player.currVelocity.magnitude : 1f) + 0.01f;

        bobPosition.x = (curveCos * bobLimit.x * (player.isGrounded ? 1f : 0)) - (player.horizontalInput * travelLimit.x);
        bobPosition.y = (curveSin * bobLimit.y) - (player.verticalInput * travelLimit.y);
        bobPosition.z = -(player.verticalInput * travelLimit.z);
    }*/
}
