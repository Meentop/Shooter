using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : AgentState
{
    public override Steering GetSteering()
    {
        Steering steering = new Steering();

        steering.linear = Vector3.zero;
        steering.orientation = agent.orientation;

        return steering;
    }

    protected override void OnDisable()
    {
        
    }

    protected override void OnEnable()
    {
        anim.SetBool("Move", false);
    }
}
