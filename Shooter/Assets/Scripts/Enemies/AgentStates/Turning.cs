using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Turning : AgentState
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private AgentState nextState;

    public override Steering GetSteering()
    {
        Steering steering = new Steering();

        steering.linear = Vector3.zero;

        Vector3 direction = (player.transform.position - transform.position).normalized;
        if (direction.magnitude > 0)
        {
            float targetOrientation = Mathf.Atan2(direction.x, direction.z);
            targetOrientation *= Mathf.Rad2Deg;
            steering.orientation = targetOrientation;
        }

        return steering;
    }

    protected override void OnDisable()
    {
        agent.SetNormalRotationSpeed();
    }

    protected override void OnEnable()
    {
        agent.SetRotationSpeed(rotationSpeed);
        float dotResult = Vector3.Dot(transform.right, player.transform.position - transform.position);
        if (dotResult > 0)
            anim.SetTrigger("TurnRight");
        else
            anim.SetTrigger("TurnLeft");
    }

    public void Handle_EndTurning()
    {
        nextState.enabled = true;
        enabled = false;
    }
}
