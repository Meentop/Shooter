using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEnemyModel : MonoBehaviour
{
    [SerializeField] private JumpEnemy enemy;

    private void SetCenEndJump()
    {
        enemy.SetCenEndJump();
    }

    private void SetEndAttack()
    {
        enemy.SetEndAttack();
    }

    public void SetEndPunch()
    {
        enemy.EndPunch();
    }
}
