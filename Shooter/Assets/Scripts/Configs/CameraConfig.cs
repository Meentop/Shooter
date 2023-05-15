using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Camera/Config")]
public class CameraConfig : ScriptableObject
{
    public float sensivity;
    public float minAngle;
    public float maxAngle;
    public float aimX;
    public float aimZ;
}
