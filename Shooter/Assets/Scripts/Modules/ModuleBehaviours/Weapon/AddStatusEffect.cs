using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddStatusEffect : WeaponModuleBehaviour
{
    [SerializeField] private StatusEffect statusEffect;
    [SerializeField] private int[] value;

    public override DamageData ApplyBehaviour(DamageData damageData, InfoForWeaponModule info)
    {
        if (!damageData.AddStatusEffects.ContainsKey(statusEffect))
            damageData.AddStatusEffects.Add(statusEffect, value[info.lvl]);
        else
            damageData.AddStatusEffects[statusEffect] += value[info.lvl];
        return damageData;
    }

    public override string GetDescription(int lvl)
    {
        return $"Add {value[lvl]} {statusEffect} to enemy";
    }
}
