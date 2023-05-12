using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Movement Settings")]
public class MoveSettings : ScriptableObject
{

    [Header("Walk/Running")]
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;

    [Header("Jumping")]
    public float jumpMulti;
    public float jumpCD;
    public float airMulti;

    [Header("Crouching")]
    public float crouchSpeed;
    public float yScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Misc")]
    public float playerHeight;

}