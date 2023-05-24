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

    public float HP;

    private float _maxHP;
    private float _target = 1;
    private bool _isDead = false;

    private void Start()
    {
        _maxHP = HP;
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
            var penetratedDamage = Mathf.Max(damageData.Damage - armor, 0);
            HP -= penetratedDamage;
            UpdateHealthBar(_maxHP, HP);

            Debug.Log(name + " HP = " + HP, gameObject);

            if (damageData.Hit.transform != null && damageData.Hit.transform.GetComponent<Enemy>())
            {
                DamageUI.Instance.AddText((int)penetratedDamage, damageData.Hit.transform, canvas);
            }

            if (HP <= 0)
            {
                executeOnHPBelowZero.ExecuteAll();
                _isDead = true;
            }
            else
                executeOnGetDamage.ExecuteAll();
        }
    }

    private void UpdateHealthBar(float maxHP, float currentHP)
    {
        if (healthBarSprite == null)
            return;

        _target = currentHP / maxHP;
        healthBarSprite.transform.localScale = new Vector3(currentHP / maxHP, 1, 1);
    }
}
