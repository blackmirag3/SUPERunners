using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Sway Settings")]
public class WeaponSwaySettings : ScriptableObject
{

    [Header("Sway Settings")]
    public float swaySmoothing;
    public float tiltMultiplier;
    public float rotMultiplier;
    public float maxRot;
    public bool tiltX;
    public bool tiltY;
    public bool tiltZ;

}
