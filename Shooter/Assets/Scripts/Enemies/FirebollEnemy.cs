using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebollEnemy : Enemy
{
    [SerializeField] private float maxShootDistance;

    [SerializeField] private Fireboll fireboll;

    [SerializeField] private Transform shootPoint;

    protected override void Start()
    {
        base.Start();    
    }

    protected override void Update()
    {
        base.Update();   
        bool playerInView = !Physics.Raycast(shootPoint.position, (player.position - shootPoint.position).normalized, Vector3.Distance(shootPoint.position, player.position), LayerMask.GetMask("Solid"));
        float distance = Vector3.Distance(player.position, transform.position);
        if (playerInView && !isAttacking || distance < maxShootDistance)
        {
            anim.SetTrigger("Attack");
            isAttacking = true;
        }
        agent.enabled = !playerInView || distance >= maxShootDistance;
        if(!agent.enabled)
            RotateToPlayer();
        if ((!playerInView && !isAttacking) || distance >= maxShootDistance)
        {
            agent.SetDestination(player.position);
        }
        anim.SetBool("Move", (!playerInView && !isAttacking) || distance >= maxShootDistance);
        print(distance);
        print(!playerInView || distance >= maxShootDistance);
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