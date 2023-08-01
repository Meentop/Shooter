using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerCondition : Condition
{
    [SerializeField] private float reloadTime;
    private float timer = 0f;

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public override bool Test()
    {
        if (timer <= 0)
        {
            timer = reloadTime;
            return true;
        }
        return false;
    }
}
