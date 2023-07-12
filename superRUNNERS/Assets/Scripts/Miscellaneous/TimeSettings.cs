using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Time Settings")]
public class TimeSettings : ScriptableObject
{
    [Header("Time")]
    public float timeSens;
    public float scaleLimit; 
    public float velocityThreshold;
}
