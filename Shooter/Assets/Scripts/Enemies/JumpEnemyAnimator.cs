using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEnemyAnimator : MonoBehaviour
{
    [SerializeField] private Turning turning;
    [SerializeField] private Punch punch;
    [SerializeField] private JumpAttack attack;

    public void Handle_EndTurning()
    {
        turning.Handle_EndTurning();
    }

    public void Handle_EndPunch()
    {
        punch.Handle_EndPunch();
    }

    public void Handle_Jump()
    {
        attack.Handle_Jump();
    }

    public void Handle_Landing()
    {
        attack.Handle_Landing();
    }

    public void Handle_EndAttack()
    {
        attack.Handle_EndAttack();
    }
}
