using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class AngleToPlayerCondition : Condition
{
    [SerializeField] private Relation numberIs;
    [SerializeField] private int angle;

    public enum Relation 
    {
        Greater,
        Less
    }

    public override bool Test()
    {
        switch (numberIs) 
        { 
            case Relation.Greater:
                return GetAngleToPlayer() > angle;
            case Relation.Less:
                return GetAngleToPlayer() < angle;
            default:
                return false;
        }
    }

    protected float GetAngleToPlayer()
    {
        Vector3 toPlayer = (player.transform.position - transform.position).normalized;
        Vector3 toPlayerXZ = new Vector3(toPlayer.x, 0, toPlayer.z);
        Vector3 forwardXZ = new Vector3(transform.forward.x, 0, transform.forward.z);
        return Vector3.Angle(toPlayerXZ, forwardXZ);
    }
}
