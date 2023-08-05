using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCritDamage : WeaponModuleBehaviour
{
    [SerializeField] private int addDamageMultiplier;

    public override DamageData ApplyBehaviour(DamageData damageData, InfoForWeaponModule info)
    {
        damageData.AddCritDamage(addDamageMultiplier);
        return damageData;
    }
}
