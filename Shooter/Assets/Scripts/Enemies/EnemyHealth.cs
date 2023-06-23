using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : BaseDamageReceiver
{
    [SerializeField] private Transform hPBarCanvas;
    [SerializeField] private StatusEffectsConfig statusEffectsConfig;
    [SerializeField] private StatusEffectUI statusEffectPrefab;
    [SerializeField] private Transform statusEffectParent;
    [SerializeField] private GameObject statsuEffectOnHPBar;

    private Dictionary<StatusEffect, int> statusEffects = new Dictionary<StatusEffect, int>();

    protected override void Start()
    {
        statusEffects.Add(StatusEffect.Poison, 10);
        statusEffects.Add(StatusEffect.Burn, 5);
        base.Start();
    }

    private void LateUpdate()
    {
        if (hPBarCanvas != null)
        {
            Vector3 directionToCamera = hPBarCanvas.position - Camera.main.transform.position;
            hPBarCanvas.rotation = Quaternion.LookRotation(directionToCamera, Vector3.up);
        }
    }

    public override void OnGetDamage(DamageData damageData)
    {
        base.OnGetDamage(damageData);
        if (damageData.Hit.transform != null && damageData.Hit.transform.GetComponent<Enemy>())
            DamageUI.Instance.AddText((int)damageData.Damage, damageData.Hit.transform, hPBarCanvas.GetComponent<RectTransform>());
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
}
