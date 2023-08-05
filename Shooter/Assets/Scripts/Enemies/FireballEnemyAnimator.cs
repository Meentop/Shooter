using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballEnemyAnimator : MonoBehaviour
{
    [SerializeField] private Shooting shooting;

    public void Handle_Shoot()
    {
        shooting.Handle_Shoot();
    }
}
