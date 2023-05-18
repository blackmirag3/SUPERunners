using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Movement Settings")]
public class WeaponMovementSettings : ScriptableObject
{

    [Header("Sway Settings")]
    public bool isSwayEnabled;
    public float swaySmoothing;
    public float tiltMultiplier;
    public float rotMultiplier;
    public float maxRot;
    public bool tiltX;
    public bool tiltY;
    public bool tiltZ;

    [Header("Bob Settings")]
    public bool isBobEnabled;
}
