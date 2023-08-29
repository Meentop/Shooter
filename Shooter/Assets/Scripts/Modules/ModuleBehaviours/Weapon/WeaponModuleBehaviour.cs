using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class WeaponModuleBehaviour : ModuleBehaviour
{
    public abstract DamageData ApplyBehaviour(DamageData damageData, InfoForWeaponModule info);
}
