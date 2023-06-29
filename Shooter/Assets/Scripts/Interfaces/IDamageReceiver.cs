using UnityEngine;
using System.Collections.Generic;

public interface IDamageReceiver
{
    void GetDamage(DamageData damageData);
}

public class DamageData
{
    public int Damage = 0;
    public Dictionary<StatusEffect, int> AddStatusEffects = new Dictionary<StatusEffect, int>();
    public Dictionary<StatusEffect, int> StatusEffectsDamage = new Dictionary<StatusEffect, int>();
}

