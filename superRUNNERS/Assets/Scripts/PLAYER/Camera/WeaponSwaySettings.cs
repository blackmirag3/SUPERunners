using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Sway Settings")]
public class WeaponSwaySettings : ScriptableObject
{

    [Header("Sway Settings")]
    [SerializeField] public float swaySmoothing;
    [SerializeField] public float swayMultiplier;
}
