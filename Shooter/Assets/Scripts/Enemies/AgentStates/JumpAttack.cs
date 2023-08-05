using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : AgentState
{
    [SerializeField] private float speed, accel;
    [SerializeField] private AgentState nextState;

    private AnimState _animState;

    public enum AnimState
    {
        Prepare,
        Jump,
        Landing
    }

    public override Steering GetSteering()
    {
        Steering steering = new Steering();

        switch (_animState)
        {
            case AnimState.Prepare:
                steering.linear = Vector3.zero;
                Vector3 direction = (player.transform.position - transform.position).normalized;
                if (direction.magnitude > 0)
                {
                    float targetOrientation = Mathf.Atan2(direction.x, direction.z);
                    targetOrientation *= Mathf.Rad2Deg;
                    steering.orientation = targetOrientation;
                }
                break;
            case AnimState.Jump:
                steering.linear = transform.forward;
                steering.orientation = agent.orientation;
                break;
            case AnimState.Landing:
                steering.linear = Vector3.zero;
                steering.orientation = agent.orientation;
                break;
        }

        return steering;
    }

    public void Handle_Jump()
    {
        _animState = AnimState.Jump;
        agent.SetMaxSpeed(speed);
        agent.SetAcceleration(accel);
    }

    public void Handle_Landing()
    {
        _animState = AnimState.Landing;
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
        anim.SetTrigger("Jump");
        _animState = AnimState.Prepare;
    }
}
