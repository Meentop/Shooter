using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PullableConfig", menuName = "ScriptableObjects/PullableConfig")]
public class PullableConfig : ScriptableObject
{
    public float randomInsideSphereSize;
    public float XZMin;
    public float XZMax;
    public float YMin;
    public float YMax;
}
