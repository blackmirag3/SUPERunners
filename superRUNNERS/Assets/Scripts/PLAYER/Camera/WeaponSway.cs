using System;
using UnityEngine;

public class WeaponSway : MonoBehaviour {

    [SerializeField] private WeaponSwaySettings settings;
    [SerializeField] private PlayerCam playerCam;

    [Header("Sway Settings")]
    [SerializeField] private float swaySmoothing;
    [SerializeField] private float tiltMultiplier;
    [SerializeField] private float rotMultiplier;
    [SerializeField] private float maxRot;
    [SerializeField] private bool tiltX;
    [SerializeField] private bool tiltY;
    [SerializeField] private bool tiltZ;

    private void Start()
    {
        InitialiseSettings();
    }

    private void Update()
    {
        MoveSway();
        TiltSway();
    }

    private void InitialiseSettings()
    {
        swaySmoothing = settings.swaySmoothing;
        tiltMultiplier = settings.tiltMultiplier;
        rotMultiplier = settings.rotMultiplier;
        maxRot = settings.maxRot;
        tiltX = settings.tiltX;
        tiltY = settings.tiltY;
        tiltZ = settings.tiltZ;
    }

    private void MoveSway()
    {
        Quaternion rotationX = Quaternion.AngleAxis(playerCam.mouseY * tiltMultiplier, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(-playerCam.mouseX * tiltMultiplier, Vector3.up);
        Quaternion targetRotation = rotationX * rotationY;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, swaySmoothing * Time.deltaTime);
    }

    private void TiltSway()
    {
        float rotationY = Mathf.Clamp(playerCam.mouseX * rotMultiplier, -maxRot, maxRot);
        float rotationX = Mathf.Clamp(playerCam.mouseY * rotMultiplier, -maxRot, maxRot);
        Quaternion targetRotation = Quaternion.Euler(new Vector3(
            tiltX ? -rotationX : 0,
            tiltY ? rotationY : 0,
            tiltZ ? rotationY : 0
            ));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, swaySmoothing * Time.deltaTime);
    }
}
