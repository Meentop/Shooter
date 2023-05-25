using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemy : Enemy
{
    [SerializeField] private float attackRange;

    [SerializeField] private float maxRadiansDelta;

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

    public void RotateToPlayer()
    {
        Vector3 playerPosXZ = new Vector3(player.position.x, 0, player.position.z);
        Vector3 transformPosXZ = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 targetDirection = playerPosXZ - transformPosXZ;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, maxRadiansDelta, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
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
