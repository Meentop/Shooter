using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInViewCondition : Condition
{
    [SerializeField] private Transform lookPoint;

    public override bool Test()
    {
        return !Physics.Raycast(lookPoint.position, (player.transform.position - lookPoint.position).normalized, Vector3.Distance(lookPoint.position, player.transform.position), LayerMask.GetMask("Solid"));
    }
}
