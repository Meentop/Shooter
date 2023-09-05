using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCharacteristics 
{
    public PlayerCharacteristics(PlayerCharacteristics movement)
    {
        maxHealth = movement.maxHealth;
        movementSpeed = movement.movementSpeed;
        groundDrag = movement.groundDrag;
        jumpStrength = movement.jumpStrength;
        airMultiplayer = movement.airMultiplayer;
        maxSlopeAngle = movement.maxSlopeAngle;
        dashStrength = movement.dashStrength;
        dashDuration = movement.dashDuration;
        dashReloadTime = movement.dashReloadTime;
        dashCharges = movement.dashCharges;
        bloodyActiveSkill = movement.bloodyActiveSkill;
        activeSkillBloodyPrice = movement.activeSkillBloodyPrice;
    }

    public int maxHealth;
    public float movementSpeed;
    public float groundDrag;
    public float jumpStrength;
    public float airMultiplayer; 
    public float maxSlopeAngle;
    public float dashStrength;
    public float dashDuration;
    public float dashReloadTime;
    public int dashCharges;
    public bool bloodyActiveSkill;
    public int activeSkillBloodyPrice;
}
