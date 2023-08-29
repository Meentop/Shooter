using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyStatusEffectDamage : WeaponModuleBehaviour
{
    [SerializeField] private StatusEffect statusEffect;
    [SerializeField] private float[] statusEffectDamage;

    public override DamageData ApplyBehaviour(DamageData damageData, InfoForWeaponModule info)
    {
        if (!damageData.AddStatusEffects.ContainsKey(statusEffect))
            damageData.AddStatusEffects.Add(statusEffect, (int)(damageData.Damage * statusEffectDamage[info.lvl]));
        else
            damageData.AddStatusEffects[statusEffect] += (int)(damageData.Damage * statusEffectDamage[info.lvl]);
        damageData.SetDamage(0);
        return damageData;
    }

    public override string GetDescription(int lvl)
    {
        return $"Weapon add {statusEffectDamage[lvl] * 100}% {statusEffect} from base damage. Weapon can't deal base damage";
    }
}
