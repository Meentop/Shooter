using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCondition : Condition
{
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;

    public override bool Test()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance >= minDistance && distance <= maxDistance;
    }
}
