using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BobSettings")]
public class BobSettings : ScriptableObject
{

    [Header("Bob Settings")]
    public bool isBobEnabled;
    [Range(0, 0.01f)] public float baseAmplitude;
    [Range(0, 30)] public float baseFrequency;
    public float baseSpeed;
    public float stabAmount;
}
