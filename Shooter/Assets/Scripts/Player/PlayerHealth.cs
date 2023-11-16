using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : BaseDamageReceiver
{
    [SerializeField] private Text _health;
    [SerializeField] protected PostProcessingController postController;

    protected override void Start()
    {
        
    }

    protected override void UpdateHealthBar()
    {
        base.UpdateHealthBar();
        _health.text = curHP.ToString() + " / " + maxHP.ToString();
    }

    public int GetHealth() => curHP;

    public int GetMaxHealth() => maxHP;

    public void SetCurHealth(int health)
    {
        curHP = health;
        UpdateHealthBar();
    }

    public void SetMaxHealth(int maxHealth)
    {
        maxHP = maxHealth;
        UpdateHealthBar();
    }

    public void SetCurHealthToMax()
    {
        curHP = maxHP;
        UpdateHealthBar();
    }

    public override void GetDamage(DamageData damageData)
    {
        base.GetDamage(damageData);
        postController.SetDamageBool(true);
    }
}
