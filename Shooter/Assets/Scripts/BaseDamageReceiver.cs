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
    private float _barSizeRatio = 1;
    protected bool _isDead = false;
    protected Vector2 _startSizeDelta;

    protected virtual void Start()
    {
        curHP = maxHP;
        _startSizeDelta = healthBar.sizeDelta;
        UpdateHealthBar();
    }

    protected virtual void Update()
    { 
        afterHealthBarImage.transform.localScale = new Vector3(Mathf.MoveTowards(afterHealthBarImage.transform.localScale.x, _barSizeRatio, UIReduceSpeed * Time.deltaTime), 1, 1);
    }

    public virtual void GetDamage(DamageData damageData)
    {
        if (!_isDead)
        {
            curHP -= damageData.Damage;
            UpdateHealthBar();

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
        UpdateHealthBar();
    }

    protected virtual void UpdateHealthBar()
    {
        if (healthBar == null)
            return;

        _barSizeRatio = (float)curHP / maxHP;
        healthBar.sizeDelta = new Vector3(_barSizeRatio * _startSizeDelta.x, _startSizeDelta.y);
        print(_barSizeRatio.ToString() + " * " + _startSizeDelta.x.ToString());
    }
}
