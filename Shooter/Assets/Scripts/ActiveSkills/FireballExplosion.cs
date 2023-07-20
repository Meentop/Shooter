using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FireballExplosion : MonoBehaviour
{
    [SerializeField] private int fireDamage;
    [SerializeField] private float radius;

    private void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Enemy"));
        BaseDamageReceiver[] healths = colliders.Select(collider => collider.GetComponentInParent<BaseDamageReceiver>()).Distinct().ToArray();
        foreach (var health in healths)
        {
            Dictionary<StatusEffect, int> effect = new Dictionary<StatusEffect, int> { { StatusEffect.Burn, fireDamage } };
            health.GetDamage(new DamageData(addStatusEffects: effect));
        }
    }
}
