using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Movement Settings", menuName = "Settings/Movement Settings")]
public class MoveSettings : ScriptableObject
{

    [Header("Walk/Running")]
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;
    public float slideDrag;
    public float wallDrag;

    [Header("Jumping")]
    public float jumpMulti;
    public float jumpCD;
    public float airMulti;

    [Header("Crouching")]
    public float crouchSpeed;
    public float yScale;

    [Header("Sliding")]
    public float slideBoostResetTime;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintToggleKey = KeyCode.LeftShift;
    public KeyCode sprintHoldKey;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Misc")]
    public float playerHeight;

}
