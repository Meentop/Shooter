using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebollEnemy : Enemy
{
    [SerializeField] private Fireboll fireboll;

    [SerializeField] private Transform shootPoint;

    protected override void Start()
    {
        base.Start();    
    }

    protected override void Update()
    {
        base.Update();
        RotateToPlayer();
        bool playerInView = !Physics.Raycast(shootPoint.position, (player.position - shootPoint.position).normalized, Vector3.Distance(shootPoint.position, player.position), LayerMask.GetMask("Solid"));
        if (playerInView && !isAttacking)
        {
            anim.SetTrigger("Attack");
            isAttacking = true;
        }
        agent.enabled = !playerInView;
        if(!playerInView && !isAttacking)
        {
            agent.SetDestination(player.position);
        }
        anim.SetBool("Move", !playerInView && !isAttacking);
    }

    public void Shoot()
    {
        Instantiate(fireboll.gameObject, shootPoint.position, shootPoint.rotation);
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
}
