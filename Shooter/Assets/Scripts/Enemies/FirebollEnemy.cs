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
        if (!isAttacking || distance < maxShootDistance)
        {
            if (playerInView)
            {
                anim.SetTrigger("Attack");
                isAttacking = true;
            }
        }
        canMove = (!playerInView && !isAttacking) || distance >= maxShootDistance;
        if(!canMove)
            RotateToPlayer();
        anim.SetBool("Move", (!playerInView && !isAttacking) || distance >= maxShootDistance);
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
