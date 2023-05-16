using System;
using UnityEngine;

public class WeaponSway : MonoBehaviour {

    [SerializeField] private WeaponSwaySettings settings;
    [SerializeField] private PlayerCam playerCam;

    [Header("Sway Settings")]
    [SerializeField] private float swaySmoothing;
    [SerializeField] private float swayMultiplier;

    private void Start()
    {
        swaySmoothing = settings.swaySmoothing;
        swayMultiplier = settings.swayMultiplier;
    }

    private void Update()
    {
        Quaternion rotationX = Quaternion.AngleAxis(playerCam.mouseY * swayMultiplier, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(-playerCam.mouseX * swayMultiplier, Vector3.up);
        Quaternion targetRotation = rotationX * rotationY;

        // rotate 
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, swaySmoothing * Time.deltaTime);
    }
}
