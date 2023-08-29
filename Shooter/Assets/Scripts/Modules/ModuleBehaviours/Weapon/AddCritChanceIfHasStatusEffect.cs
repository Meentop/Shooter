using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCritChanceIfHasStatusEffect : WeaponModuleBehaviour
{
    [SerializeField] private StatusEffect statusEffect;
    [SerializeField] private int[] addCritChance;

    public override DamageData ApplyBehaviour(DamageData damageData, InfoForWeaponModule info)
    {
        if (info.enemyStatusEffects.ContainsKey(statusEffect))
            damageData.AddCritChance(addCritChance[info.lvl]);
        return damageData;
    }

    public override string GetDescription(int lvl)
    {
        return $"+{addCritChance[lvl]}% critical hit chance if enemy has {statusEffect}";
    }
}
