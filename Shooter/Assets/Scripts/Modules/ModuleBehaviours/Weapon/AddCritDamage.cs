using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCritDamage : WeaponModuleBehaviour
{
    [SerializeField] private float[] addDamageMultiplier;

    public override DamageData ApplyBehaviour(DamageData damageData, InfoForWeaponModule info)
    {
        damageData.AddCritDamage(addDamageMultiplier[info.lvl]);
        return damageData;
    }

    public override string GetDescription(int lvl)
    {
        return $"+{addDamageMultiplier[lvl] * 100}% crit damage";
    }
}
