using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : BaseDamageReceiver
{
    [SerializeField] private Text _health;

    protected override void Start()
    {
        
    }

    public void AddMaxHealth(int add)
    {
        float coefficient = curHP / (float)maxHP;
        maxHP += add;
        curHP = Mathf.RoundToInt(maxHP * coefficient);
        UpdateHealthBar();
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
        _startSizeDelta = healthBar.sizeDelta;
        UpdateHealthBar();
    }
}
