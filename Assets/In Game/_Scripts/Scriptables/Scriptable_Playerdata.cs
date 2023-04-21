using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Scriptable_Playerdata")]
public class Scriptable_Playerdata : ScriptableObject
{
    [Header("Functional toggle"), Tooltip("To on and off some player functionality")]
    public bool isActive = true;
    public bool canWalk = true;
    public bool canSprint = true;
    public bool canInteract = true;
    public bool canAirMove = true;
    public bool canJump = true;
    public bool canStepUp = true;


    [Header("MOVE")]
    [Header("Walk")]
    public float walkSpeed;


    [Header("Acceleration and multiplier")]
    public float acceleration;
    public float airMovementMultiplier;
    public float frictionWhenStoping;
    public float groundMovementMultiplier;

    [Header("Sprint")]
    public float sprintSpeed;
    public KeyCode sprintKey;

    [Header("CAMERA AND ROTATION")]
    public float camSensi;
    public float upperLookLimit;
    public float lowerLookLimit;

    [Header("JUMPING")]
    public float jumpHeight;
    public KeyCode jumpKey;
    public bool allowJumpButtonHold;
    public float jumpCooldown;

    [Header("GROUND CHECK")]
    public float groundCheckDistance;
    public Transform sphereCastPosition;
    public float sphereRadius;
    public LayerMask groundLayer;

    [Header("Gravity")]
    public float gravityMultiplier;
    public float maxFallingSpeed;

    [Header("Stair Step Up")]
    public float maxStepHeight;
    public float stepUpSpeed;
    public Transform stepRayUpper;
    public Transform stepRayLower;
    [Range(.1f, .5f)] public float lowerRayDetectRange;
    [Range(.1f, .5f)] public float upperRayDetectRange;

}
