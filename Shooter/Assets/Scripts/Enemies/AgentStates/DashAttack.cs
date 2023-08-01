using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : AgentState
{
    [SerializeField] private float dashSpeed;
    [SerializeField] private AgentState nextState;

    private AnimState _animState;

    public enum AnimState
    {
        Start,
        Dash,
        EndDash
    }

    public override Steering GetSteering()
    {
        Steering steering = new Steering();

        switch (_animState)
        {
            case AnimState.Start:
                steering.linear = Vector3.zero;
                Vector3 direction = (player.transform.position - transform.position).normalized;
                if (direction.magnitude > 0)
                {
                    float targetOrientation = Mathf.Atan2(direction.x, direction.z);
                    targetOrientation *= Mathf.Rad2Deg;
                    steering.orientation = targetOrientation;
                }
                break;
            case AnimState.Dash:
                steering.linear = transform.forward;
                steering.orientation = agent.orientation;
                break;
            case AnimState.EndDash:
                steering.linear = Vector3.zero;
                steering.orientation = agent.orientation;
                break;
        }

        return steering;
    }

    public void Handle_Dash()
    {
        _animState = AnimState.Dash;
        agent.SetMaxSpeed(dashSpeed);
        agent.SetAcceleration(100000);
    }

    public void Handle_EndDash()
    {
        _animState = AnimState.EndDash;
        agent.SetNormalMaxSpeed();
        agent.SetNormalAcceleration();
    }

    public void Handle_EndAttack()
    {
        nextState.enabled = true;
        enabled = false;
    }

    protected override void OnDisable()
    {

    }

    protected override void OnEnable()
    {
        anim.SetBool("Move", false);
        anim.SetTrigger("Attack");
        _animState = AnimState.Start;
    }
}
