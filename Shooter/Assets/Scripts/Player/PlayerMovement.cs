using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerMovement 
{
    public PlayerMovement(PlayerMovement movement)
    {
        movementSpeed = movement.movementSpeed;
        groundDrag = movement.groundDrag;
        jumpStrength = movement.jumpStrength;
        airMultiplayer = movement.airMultiplayer;
        maxSlopeAngle = movement.maxSlopeAngle;
        dashStrength = movement.dashStrength;
        dashDuration = movement.dashDuration;
        dashReloadTime = movement.dashReloadTime;
        dashCharges = movement.dashCharges;
    }

    public float movementSpeed;
    public float groundDrag;
    public float jumpStrength;
    public float airMultiplayer; 
    public float maxSlopeAngle;
    public float dashStrength;
    public float dashDuration;
    public float dashReloadTime;
    public int dashCharges;
}
