using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEnemy : Enemy
{
    [SerializeField] private float maxJumpDistance, minJumpDistance, punchDistance, punchReloadTime;

    private bool _isRotating, _canPunch = true;

    private Vector3 _playerForRotating;

    protected override void Update()
    {
        base.Update();
        float distance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.position.x, 0, player.position.z));
        agent.enabled = distance > maxJumpDistance;
        anim.SetBool("Move", distance > maxJumpDistance);
        if (agent.enabled)
            agent.SetDestination(player.position);

        if (distance <= maxJumpDistance)
        {
            if (GetAngleToPlayer() > 45f)
            {
                if (!_isRotating)
                {
                    LeftOrRight();
                    _playerForRotating = player.position;
                    _isRotating = true;
                }
            }
        }

        if(_isRotating)
        {
            RotateToPlayer45(_playerForRotating);
        }

        if (distance <= punchDistance && GetAngleToPlayer() < 45f && _canPunch)
        {
            anim.SetTrigger("Punch");
            _canPunch = false;
            Invoke(nameof(SetCanPunch), punchReloadTime);
        }
    }

    private void LeftOrRight()
    {
        float dotResult =  Vector3.Dot(transform.right, player.position - transform.position);
        if (dotResult > 0)
            anim.SetTrigger("TurnRight");
        else
            anim.SetTrigger("TurnLeft");
        Invoke(nameof(EndRotating), 1.33f);
    }

    public void RotateToPlayer45(Vector3 player)
    {
        Vector3 playerPosXZ = new Vector3(player.x, 0, player.z);
        Vector3 transformPosXZ = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 targetDirection = playerPosXZ - transformPosXZ;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, maxRadiansDelta, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void EndRotating()
    {
        _isRotating = false;
    }

    private void SetCanPunch()
    {
        _canPunch = true;
    }
}
