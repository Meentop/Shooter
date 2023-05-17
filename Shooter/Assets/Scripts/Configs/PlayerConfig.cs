using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Config")]
public class PlayerConfig : ScriptableObject
{
    public float movementSpeed;
    public float groundDrag;
    public float jumpStrength;
    public float airMultiplayer;
    public float maxSlopeAngle;
    public float dashStrength;
    public float dashDuration;
    public float dashReloadTime;
}
