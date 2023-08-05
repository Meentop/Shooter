using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DashAttack;

public class DashEnemyAnimator : MonoBehaviour
{
    [SerializeField] private DashAttack dashAttack;

    public void Handle_Dash()
    {
        dashAttack.Handle_Dash();
    }

    public void Handle_EndDash()
    {
        dashAttack.Handle_EndDash();
    }

    public void Handle_EndAttack()
    {
        dashAttack.Handle_EndAttack();
    }
}
