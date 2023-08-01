using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : AgentState
{
    [SerializeField] private EnemyFireball fireboll;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Condition constantCondition;
    [SerializeField] private AgentState nextState;

    protected override void Update()
    {
        base.Update();
        if(!constantCondition.Test())
        {
            nextState.enabled = true;
            enabled = false;
        }
    }

    public override Steering GetSteering()
    {
        Steering steering = new Steering();

        steering.linear = Vector3.zero;

        Vector3 direction = (player.transform.position - transform.position).normalized;
        if (direction.magnitude > 0)
        {
            float targetOrientation = Mathf.Atan2(direction.x, direction.z);
            targetOrientation *= Mathf.Rad2Deg;
            steering.orientation = targetOrientation;
        }

        return steering;
    }

    public void Handle_Shoot()
    {
        EnemyFireball fireboll = ObjectPool.Instance.GetObject(this.fireboll);
        fireboll.transform.SetPositionAndRotation(shootPoint.position, Quaternion.LookRotation((player.transform.position - shootPoint.position).normalized));
    }

    protected override void OnDisable()
    {
        anim.SetBool("Shooting", false);
    }

    protected override void OnEnable()
    {
        anim.SetBool("Shooting", true);
    }
}
