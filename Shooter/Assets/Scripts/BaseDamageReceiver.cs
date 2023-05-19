using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseDamageReceiver : MonoBehaviour, IDamageReceiver
{
    [SerializeField] private ActionBase[] executeOnGetDamage;
    [SerializeField] private ActionBase[] executeOnHPBelowZero;
    [SerializeField] private Image healthBarSprite;
    [SerializeField] private Image afterHealthBarSprite;
    [SerializeField] private Transform hPBarCanvas;
    [SerializeField] private float reduceSpeed;
    [SerializeField] private float armor;

    public float HP;

    private float maxHP;
    private float _target = 1;

    private void Start()
    {
        maxHP = HP;
    }

    private void Update()
    {
        if(hPBarCanvas != null)
            hPBarCanvas.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        afterHealthBarSprite.transform.localScale = new Vector3(Mathf.MoveTowards(afterHealthBarSprite.transform.localScale.x, _target, reduceSpeed * Time.deltaTime), 1, 1);
    }

    public void OnGetDamage(DamageData damageData)
    {
        var penetratedDamage = Mathf.Max(damageData.Damage - armor, 0);
        HP -= penetratedDamage;
        UpdateHealthBar(maxHP, HP);
            
        Debug.Log(name + " HP = " + HP, gameObject);

        if (damageData.Hit.transform != null && damageData.Hit.transform.GetComponent<Enemy>())
        {
            DamageUI.Instance.AddText((int)penetratedDamage, transform.position);
        }

        if (HP <= 0)
            executeOnHPBelowZero.ExecuteAll();
        else
            executeOnGetDamage.ExecuteAll();
    }

    private void UpdateHealthBar(float maxHP, float currentHP)
    {
        if (healthBarSprite == null)
            return;

        _target = currentHP / maxHP;
        healthBarSprite.transform.localScale = new Vector3(currentHP / maxHP, 1, 1);
    }
}
