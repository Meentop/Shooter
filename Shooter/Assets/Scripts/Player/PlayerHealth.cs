using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : BaseDamageReceiver
{
    [SerializeField] private Text _health;

    public void AddMaxHealth(int add)
    {
        float coefficient = curHP / (float)maxHP;
        maxHP += add;
        curHP = Mathf.RoundToInt(maxHP * coefficient);
        UpdateHealthBar(maxHP, curHP);
    }

    protected override void UpdateHealthBar(float maxHP, float currentHP)
    {
        base.UpdateHealthBar(maxHP, currentHP);
        _health.text = currentHP.ToString() + " / " + maxHP.ToString();
    }

    public int GetPlayerHelth() => curHP;
}
