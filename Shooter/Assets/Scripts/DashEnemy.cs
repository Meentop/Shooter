using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemy : Enemy
{
    [SerializeField] private float attackRange;

    protected override void Update()
    {
        base.Update();
        if (Vector3.Distance(transform.position, player.position) <= attackRange && !isAttacking && GetAngleToPlayer() < 10)
            StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        print("start attacking");
        isAttacking = true;
        anim.SetTrigger("Attack");
        agent.enabled = false;

        yield return new WaitForSeconds(1.05f);

        print("start dash");
        rb.AddForce(transform.forward * 20f, ForceMode.Impulse);

        yield return new WaitForSeconds(0.1f);

        rb.velocity = Vector3.zero; 
        print("end dash");

        yield return new WaitForSeconds(1.05f);

        anim.transform.localRotation = Quaternion.Euler(Vector3.zero);
        anim.transform.localPosition = Vector3.zero;
        agent.enabled = true;
        isAttacking = false;
        print("end attacking");
    }
}
