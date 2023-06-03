using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEnemy : Enemy
{
    [SerializeField] private float maxRadiansDelta45, maxJumpDistance, minJumpDistance, punchDistance;

    [SerializeField] private float punchReloadTime, jumpReloadTime;

    [SerializeField] private float jumpHeight, jumpDistance;

    private bool _isRotating, _canEndJump, _isPunching;

    private Vector3 _playerForRotating;

    private float punchTimer, jumpTimer;

    protected override void Update()
    {
        base.Update();
        Timers();
        float distance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(player.position.x, 0, player.position.z));
        if (!isAttacking && !_isPunching)
        {
            if (distance > minJumpDistance && distance < maxJumpDistance)
            {
                if (GetAngleToPlayer() < 45f && jumpTimer <= 0 && punchTimer <= 0)
                    JumpAttack();
            }

            if (!_isRotating)
                SetCanMove(distance);

            if (distance <= maxJumpDistance && GetAngleToPlayer() > 45f && !_isRotating)
                Rotation();

            if (distance <= punchDistance && GetAngleToPlayer() < 45f && punchTimer <= 0)
                Punch();

            if (_isRotating)
                RotateToPlayer45(_playerForRotating); 
        }
        else
        {
            if (_canEndJump && Physics.Raycast(transform.position, Vector3.down, 3f, LayerMask.GetMask("Solid")))
                EndJump();
        }
    }

    private void Timers()
    {
        if (jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;
        }
        if (punchTimer > 0)
        {
            punchTimer -= Time.deltaTime;
        }
    }

    private void SetCanMove(float distance)
    {
        canMove = distance > maxJumpDistance || (distance < minJumpDistance && distance > punchDistance);
        if (!isAttacking)
            anim.SetBool("Move", distance > maxJumpDistance || (distance < minJumpDistance && distance > punchDistance));
    }

    private void Rotation()
    {
        LeftOrRight();
        _playerForRotating = player.position;
        _isRotating = true;
        rb.velocity = Vector3.zero;
    }

    private void LeftOrRight()
    {
        float dotResult =  Vector3.Dot(transform.right, player.position - transform.position);
        if (dotResult > 0)
            anim.SetTrigger("TurnRight");
        else
            anim.SetTrigger("TurnLeft");
        Invoke(nameof(EndRotating), 1f);
    }

    public void RotateToPlayer45(Vector3 player)
    {
        Vector3 playerPosXZ = new Vector3(player.x, 0, player.z);
        Vector3 transformPosXZ = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 targetDirection = playerPosXZ - transformPosXZ;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, maxRadiansDelta45, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void JumpAttack()
    {
        Vector3 playerPosXZ = new Vector3(player.position.x, 0, player.position.z);
        Vector3 transformPosXZ = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 targetDirection = playerPosXZ - transformPosXZ;
        transform.rotation = Quaternion.LookRotation(targetDirection);

        rb.velocity = Vector3.zero;
        rb.AddForce((Vector3.up * jumpHeight + (player.position - transform.position).normalized).normalized * jumpDistance, ForceMode.VelocityChange);
        anim.SetTrigger("Jump");
        isAttacking = true;
        jumpTimer = jumpReloadTime;
    }

    private void Punch()
    {
        anim.SetTrigger("Punch");
        rb.velocity = Vector3.zero;
        punchTimer = punchReloadTime;
        _isPunching = true;
    }

    public void EndPunch()
    {
        _isPunching = false;
    }

    private void EndJump()
    {
        anim.SetTrigger("EndJump");
        _canEndJump = false;
    }

    private void EndRotating()
    {
        _isRotating = false;
    }

    public void SetEndAttack()
    {
        isAttacking = false;
    }

    public void SetCenEndJump()
    {
        _canEndJump = true;
    }
}
