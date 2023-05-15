using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Config")]
public class PlayerConfig : ScriptableObject
{
    public float movementSpeed;
    public float jumpStrength;
    public float checkOnGroundRadius;
    public float dashStrength;
    public float dashDuration;
    public float dashReloadTime;
}
