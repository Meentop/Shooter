using UnityEngine;

public interface IDamageReceiver
{
    void OnGetDamage(DamageData damageData);
}

public struct DamageData
{
    public readonly float Damage;
    public readonly RaycastHit Hit;

    public DamageData(float damage, RaycastHit hit)
    {
        Damage = damage;
        Hit = hit;
    }
}

