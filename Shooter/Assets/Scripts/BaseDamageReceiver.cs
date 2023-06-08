using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseDamageReceiver : MonoBehaviour, IDamageReceiver
{
    [SerializeField] private ActionBase[] executeOnGetDamage;
    [SerializeField] private ActionBase[] executeOnHPBelowZero;
    [SerializeField] private RectTransform canvas;
    [SerializeField] private Image healthBarSprite;
    [SerializeField] private Image afterHealthBarSprite;
    [SerializeField] private Transform hPBarCanvas;
    [SerializeField] private float reduceSpeed;
    [SerializeField] private float armor;

    protected int curHP;

    [SerializeField] protected int maxHP;
    private float _target = 1;
    private bool _isDead = false;

    protected virtual void Start()
    {
        curHP = maxHP;
        UpdateHealthBar(maxHP, curHP);
    }

    private void Update()
    { 
        afterHealthBarSprite.transform.localScale = new Vector3(Mathf.MoveTowards(afterHealthBarSprite.transform.localScale.x, _target, reduceSpeed * Time.deltaTime), 1, 1);
    }

    private void LateUpdate()
    {
        if (hPBarCanvas != null)
        {
            Vector3 directionToCamera = hPBarCanvas.position - Camera.main.transform.position;
            hPBarCanvas.rotation = Quaternion.LookRotation(directionToCamera, Vector3.up);
        }
    }

    public void OnGetDamage(DamageData damageData)
    {
        if (!_isDead)
        {
            curHP -= (int)damageData.Damage;
            UpdateHealthBar(maxHP, curHP);

            if (damageData.Hit.transform != null && damageData.Hit.transform.GetComponent<Enemy>())
                DamageUI.Instance.AddText((int)damageData.Damage, damageData.Hit.transform, canvas);

            if (curHP <= 0)
            {
                executeOnHPBelowZero.ExecuteAll();
                _isDead = true;
            }
            else
                executeOnGetDamage.ExecuteAll();
        }
    }

    protected virtual void UpdateHealthBar(float maxHP, float currentHP)
    {
        if (healthBarSprite == null)
            return;

        _target = currentHP / maxHP;
        healthBarSprite.transform.localScale = new Vector3(currentHP / maxHP, 1, 1);
    }
}
