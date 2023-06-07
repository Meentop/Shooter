using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [HideInInspector] public float damagePower = 1;

    public void AddDamagePower(float pescentage)
    {
        float damageIncrease = damagePower * pescentage;
        damagePower += damageIncrease;
    }
}
