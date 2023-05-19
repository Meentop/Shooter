using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemyModel : MonoBehaviour
{
    [SerializeField] private DashEnemy dashEnemy;

    public void StartAttack()
    {
        dashEnemy.StartAttack();
    }

    public void StartDash()
    {
        dashEnemy.StartDash();
    }

    public void EndDash()
    {
        dashEnemy.EndDash();
    }

    public void EndAttack()
    {
        dashEnemy.EndAttack();
    }
}
