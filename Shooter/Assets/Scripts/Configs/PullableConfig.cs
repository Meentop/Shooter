using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pullable/Config")]
public class PullableConfig : ScriptableObject
{
    public float randomInsideSphereSize;
    public float XZMin;
    public float XZMax;
    public float YMin;
    public float YMax;
}
