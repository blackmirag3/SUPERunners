using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BobSettings")]
public class BobSettings : ScriptableObject
{

    [Header("Bob Settings")]
    public bool enable;
    [Range(0, 0.1f)] public float amplitude;
    [Range(0, 30)] public float frequency;
    public float toggleSpeed;
    public float stabAmount;
}
