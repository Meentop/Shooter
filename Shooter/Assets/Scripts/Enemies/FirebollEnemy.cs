using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebollEnemy : Enemy
{
    [SerializeField] private float maxShootDistance;

    [SerializeField] private EnemyFireball fireboll;

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
        EnemyFireball fireboll = ObjectPool.Instance.GetObject(this.fireboll);
        fireboll.transform.position = shootPoint.position;
        fireboll.transform.rotation = Quaternion.LookRotation((player.position - shootPoint.position).normalized);
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
}
