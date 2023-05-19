using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemy : Enemy
{
    [SerializeField] private float attackRange;
    private bool _isDashing;

    protected override void Update()
    {
        base.Update();
        if (Vector3.Distance(transform.position, player.position) <= attackRange && !isAttacking && GetAngleToPlayer() < 10)
        {
            anim.SetTrigger("Attack");
            isAttacking = true;
        }
    }

    public void StartAttack()
    {      
        agent.enabled = false;
    }

    public void StartDash()
    {
        rb.AddForce(transform.forward * 20f, ForceMode.Impulse);
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
