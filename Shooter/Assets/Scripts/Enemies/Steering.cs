using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering 
{
    public float orientation;
    public Vector3 linear;
    public Steering()
    {
        orientation = 0.0f;
        linear = new Vector3();
    }
}
