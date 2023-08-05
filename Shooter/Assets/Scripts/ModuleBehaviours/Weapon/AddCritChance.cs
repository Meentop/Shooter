using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCritChance : WeaponModuleBehaviour
{
    [SerializeField] private int addCritChance;

    public override DamageData ApplyBehaviour(DamageData damageData, InfoForWeaponModule info)
    {
        damageData.AddCritChance(addCritChance);
        return damageData;
    }
}
