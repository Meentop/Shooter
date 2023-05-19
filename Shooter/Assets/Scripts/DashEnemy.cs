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
        {
            anim.SetTrigger("Attack");
            isAttacking = true;
        }
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

    public void ResetTransform()
    {
        //StartCoroutine(ResetTr());
    }

    IEnumerator ResetTr()
    {
        anim.applyRootMotion = false;
        anim.transform.localRotation = Quaternion.Euler(Vector3.zero);
        anim.transform.localPosition = Vector3.zero;
        print(anim.transform.localRotation.eulerAngles);
        yield return new WaitForEndOfFrame();
        anim.applyRootMotion = false;
        print(anim.transform.localRotation.eulerAngles);
    }
}
