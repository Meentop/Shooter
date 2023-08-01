using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : AgentState
{
    [SerializeField] private AgentState nextState;

    public override Steering GetSteering()
    {
        Steering steering = new Steering();

        steering.linear = Vector3.zero;
        steering.orientation = agent.orientation;

        return steering;
    }

    public void Handle_EndPunch()
    {
        nextState.enabled = true;
        enabled = false;
    }

    protected override void OnDisable()
    {
        
    }

    protected override void OnEnable()
    {
        anim.SetTrigger("Punch");
    }
}
