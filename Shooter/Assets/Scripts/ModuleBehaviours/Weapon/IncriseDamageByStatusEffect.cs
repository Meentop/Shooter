using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncriseDamageByStatusEffect : WeaponModuleBehaviour
{
    [SerializeField] private StatusEffect statusEffect;
    [SerializeField] private float increase;

    public override DamageData ApplyBehaviour(DamageData damageData, InfoForWeaponModule info)
    {
        if (info.enemyStatusEffects.ContainsKey(statusEffect))
            damageData.SetDamage((int)(damageData.Damage * increase));
        return damageData;
    }
}