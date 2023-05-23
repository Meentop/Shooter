using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebollEnemyModel : MonoBehaviour
{
    [SerializeField] private FirebollEnemy enemy;

    public void Shoot()
    {
        enemy.Shoot();
    }

    public void EndAttack()
    {
        enemy.EndAttack();
    }
}
