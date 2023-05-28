using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemy : Enemy
{
    [SerializeField] private float attackRange;

    private bool _isRotating;

    protected override void Update()
    {
        base.Update();
        if (agent.enabled)
            agent.SetDestination(player.position);
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
        agent.enabled = false;
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
        agent.enabled = true;
        isAttacking = false;
    }
}
