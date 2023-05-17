using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDamageReceiver : MonoBehaviour, IDamageReceiver
{
    [SerializeField] private ActionBase[] executeOnGetDamage;
    [SerializeField] private ActionBase[] executeOnHPBelowZero;
    [SerializeField] private float armor;
    public float HP;
    public void OnGetDamage(DamageData damageData)
    {
        var penetratedDamage = Mathf.Max(damageData.Damage - armor, 0);
        HP -= penetratedDamage;

        Debug.Log(name + " HP = " + HP, gameObject);

        if (HP <= 0)
            executeOnHPBelowZero.ExecuteAll();
        else
            executeOnGetDamage.ExecuteAll();
    }
}
