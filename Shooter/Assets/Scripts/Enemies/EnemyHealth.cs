using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : BaseDamageReceiver
{
    [SerializeField] private Transform hPBarCanvas;
    [SerializeField] private float scaleMultiplier;
    [SerializeField] private Transform damageNumbersPoint;
    [SerializeField] private StatusEffectsConfig statusEffectsConfig;
    [SerializeField] private StatusEffectUI statusEffectPrefab;
    [SerializeField] private Transform statusEffectParent;
    [SerializeField] private GameObject statsuEffectOnHPBar;

    private Dictionary<StatusEffect, int> statusEffects = new Dictionary<StatusEffect, int>();
    private Transform _player;
    private Player _playerComponent;
    private Vector3 _startScale;

    protected override void Start()
    {
        _player = FindObjectOfType<Player>().transform;
        _playerComponent = _player.GetComponent<Player>();
        _startScale = hPBarCanvas.localScale;
        base.Start();
        StartCoroutine(UpdateStatusEffects());
    }

    private void LateUpdate()
    {
        Vector3 directionToCamera = hPBarCanvas.position - Camera.main.transform.position;
        hPBarCanvas.rotation = Quaternion.LookRotation(directionToCamera, Vector3.up);

        float currentDistance = Vector3.Distance(transform.position, _player.position);
        float targetScale = currentDistance * scaleMultiplier;
        hPBarCanvas.localScale = _startScale * targetScale;
    }

    private IEnumerator UpdateStatusEffects()
    {
        while (true)
        {
            yield return new WaitForSeconds(statusEffectsConfig.statsuEffectDeltaTime);
            Dictionary<StatusEffect, int> statusEffectsDamage = new Dictionary<StatusEffect, int>();
            if (statusEffects.ContainsKey(StatusEffect.Poison))
            {
                int damage = statusEffects[StatusEffect.Poison] / 2;
                if (statusEffects[StatusEffect.Poison] == 1)
                    damage = 1;
                statusEffects[StatusEffect.Poison] -= damage;
                statusEffectsDamage.Add(StatusEffect.Poison, damage);
                if (statusEffects[StatusEffect.Poison] < 1)
                    statusEffects.Remove(StatusEffect.Poison);
            }
            DamageData damageData = new DamageData(statusEffectsDamage: statusEffectsDamage);
            GetDamage(damageData);
            UpdateHealthBar(maxHP, curHP);
        }
    }   

    public override void GetDamage(DamageData damageData)
    {
        _playerComponent.AddDamageToActiveSkill(Mathf.Clamp(damageData.Damage, 0, curHP));
        base.GetDamage(damageData);
        if (damageData.Damage != 0)
            DamageUI.Instance.AddText(damageData.Damage, this, hPBarCanvas.GetComponent<RectTransform>(), Color.white);

        for (int i = 0; i < 5; i++)
        {
            if (damageData.AddStatusEffects.ContainsKey((StatusEffect)i))
            {
                if (!statusEffects.ContainsKey((StatusEffect)i))
                    statusEffects.Add((StatusEffect)i, damageData.AddStatusEffects[(StatusEffect)i]);
                else
                    statusEffects[(StatusEffect)i] += damageData.AddStatusEffects[(StatusEffect)i];
            }
        }
        UpdateHealthBar(maxHP, curHP);

        foreach (var statusEffect in damageData.StatusEffectsDamage)
        {
            if (!_isDead)
            {
                _playerComponent.AddDamageToActiveSkill(Mathf.Clamp(statusEffect.Value, 0, curHP));
                curHP -= statusEffect.Value; 
                UpdateHealthBar(maxHP, curHP);

                if (curHP <= 0)
                {
                    executeOnHPBelowZero.ExecuteAll();
                    _isDead = true;
                }
                else
                    executeOnGetDamage.ExecuteAll();
            }
            DamageUI.Instance.AddText(statusEffect.Value, this, hPBarCanvas.GetComponent<RectTransform>(), statusEffectsConfig.colors[(int)statusEffect.Key]);
        }
    }

    protected override void UpdateHealthBar(float maxHP, float currentHP)
    {
        base.UpdateHealthBar(maxHP, currentHP);
        foreach (Transform child in statusEffectParent)
        {
            Destroy(child.gameObject);
        }
        foreach (var statusEffect in statusEffects)
        {
            StatusEffectUI statusEffectUI = Instantiate(statusEffectPrefab, statusEffectParent);
            statusEffectUI.Init(statusEffectsConfig.sprites[(int)statusEffect.Key], statusEffect.Value, statusEffectsConfig.colors[(int)statusEffect.Key]);
        }

        foreach (Transform child in healthBar)
        {
            Destroy(child.gameObject);
        }
        foreach (var statusEffect in statusEffects)
        {
            RectTransform statusEffectHP = Instantiate(statsuEffectOnHPBar, healthBar).GetComponent<RectTransform>();
            statusEffectHP.GetComponent<Image>().color = statusEffectsConfig.colors[(int)statusEffect.Key];
            statusEffectHP.sizeDelta = new Vector2((statusEffect.Value / maxHP) * 100, 100);
        }
    }

    public Vector3 GetDamageNumbersPos()
    {
        return damageNumbersPoint.position;
    }


    public void AddStatusEffect(StatusEffect key, int value)
    {
        if (!statusEffects.ContainsKey(key))
            statusEffects.Add(key, value);
        else
            statusEffects[key] += value;
    }

    public int GetStatusEffectValue(StatusEffect key)
    {
        if (statusEffects.ContainsKey(key))
            return statusEffects[key];
        return 0;
    }

    public bool HasStatusEffect(StatusEffect statusEffect)
    {
        return statusEffects.ContainsKey(statusEffect);
    }
}
