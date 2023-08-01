using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseDamageReceiver : MonoBehaviour, IDamageReceiver
{
    [SerializeField] protected ActionBase[] executeOnGetDamage;
    [SerializeField] protected ActionBase[] executeOnHPBelowZero;
    [SerializeField] protected RectTransform healthBar;
    [SerializeField] private Image afterHealthBarImage;
    [SerializeField] private float UIReduceSpeed = 0.2f;
    [SerializeField] protected int maxHP;

    protected int curHP;    
    private float _target = 1;
    protected bool _isDead = false;
    private Vector2 _startSizeDelta;

    protected virtual void Start()
    {
        curHP = maxHP;
        _startSizeDelta = healthBar.sizeDelta;
        UpdateHealthBar(maxHP, curHP);
    }

    protected virtual void Update()
    { 
        afterHealthBarImage.transform.localScale = new Vector3(Mathf.MoveTowards(afterHealthBarImage.transform.localScale.x, _target, UIReduceSpeed * Time.deltaTime), 1, 1);
    }

    public virtual void GetDamage(DamageData damageData)
    {
        if (!_isDead)
        {
            curHP -= damageData.Damage;
            UpdateHealthBar(maxHP, curHP);

            if (curHP <= 0)
            {
                executeOnHPBelowZero.ExecuteAll();
                _isDead = true;
            }
            else
                executeOnGetDamage.ExecuteAll();
        }
    }

    public virtual void AddHealth(int health)
    {
        curHP += health;
        if (curHP > maxHP)
            curHP = maxHP;
        UpdateHealthBar(maxHP, curHP);
    }

    protected virtual void UpdateHealthBar(float maxHP, float currentHP)
    {
        if (healthBar == null)
            return;

        _target = currentHP / maxHP;
        healthBar.sizeDelta = new Vector3((currentHP / maxHP) * _startSizeDelta.x, _startSizeDelta.y);
    }
}
