using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCamSettings", menuName = "Settings/Player Cam Settings")]
public class PlayerCamSettings : ScriptableObject
{
    [Header("Cam Settings")]
    public float sensX;
    public float sensY;
    //public float sprintFov;

    [Header("Bob Settings")]
    public bool isBobEnabled;
    [Range(0, 0.01f)] public float bobAmplitude;
    [Range(0, 30)] public float bobFrequency;
    public float stabAmount;
}
