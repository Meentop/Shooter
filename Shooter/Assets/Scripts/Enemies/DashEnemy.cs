using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DashEnemy : Enemy
{
    [SerializeField] private float attackRange;

    private bool _isRotating;

    protected override void Update()
    {
        base.Update();
        if (Vector3.Distance(transform.position, player.position) <= attackRange && !isAttacking && GetAngleToPlayer() < 10)
        {
            anim.SetTrigger("Attack");
            isAttacking = true;
        }
        if (_isRotating)
            RotateToPlayer();
    }

    public void StartAttack()
    {
        canMove = false;
        transform.position = transform.position;
        _isRotating = true;
    }

    public void StartDash()
    {
        _isRotating = false;
        rb.AddForce(transform.forward * 20f, ForceMode.VelocityChange);
    }

    public void EndDash()
    {
        rb.velocity = Vector3.zero;
    }

    public void EndAttack()
    {
        canMove = true;
        isAttacking = false;
    }
}
