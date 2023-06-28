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
    private Transform player;
    private Vector3 startScale;

    protected override void Start()
    {
        statusEffects.Add(StatusEffect.Poison, 200);
        statusEffects.Add(StatusEffect.Burn, 5);
        player = FindObjectOfType<Player>().transform;
        startScale = hPBarCanvas.localScale;
        base.Start();
        StartCoroutine(UpdateStatusEffects());
    }

    private void LateUpdate()
    {
        Vector3 directionToCamera = hPBarCanvas.position - Camera.main.transform.position;
        hPBarCanvas.rotation = Quaternion.LookRotation(directionToCamera, Vector3.up);

        float currentDistance = Vector3.Distance(transform.position, player.position);
        float targetScale = currentDistance * scaleMultiplier;
        hPBarCanvas.localScale = startScale * targetScale;
    }

    private IEnumerator UpdateStatusEffects()
    {
        while (true)
        {
            yield return new WaitForSeconds(statusEffectsConfig.statsuEffectDeltaTime);
            if (statusEffects.ContainsKey(StatusEffect.Poison))
            {
                int damage = statusEffects[StatusEffect.Poison] / 2;
                if (statusEffects[StatusEffect.Poison] == 1)
                    damage = 1;
                statusEffects[StatusEffect.Poison] -= damage;
                GetDamage(new DamageData(damage));
                if (statusEffects[StatusEffect.Poison] < 1)
                    statusEffects.Remove(StatusEffect.Poison);
            }
            UpdateHealthBar(maxHP, curHP);
        }
    }   

    public override void GetDamage(DamageData damageData)
    {
        base.GetDamage(damageData);
        DamageUI.Instance.AddText(damageData.Damage, this, hPBarCanvas.GetComponent<RectTransform>());
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
}
