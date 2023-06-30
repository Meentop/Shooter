using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Modifier : MonoBehaviour, ISelectableItem
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private string title, description;
    [SerializeField] private List<ModifierBehaviour> behaviours;

    public SelectableItems ItemType => SelectableItems.Modifier;

    [System.Serializable]
    public struct Info
    {
        public Image Image;
        public Text Title;
        public Text Description;
    }

    public Modifier(Sprite sprite, string title, string description)
    {
        this.sprite = sprite;
        this.title = title;
        this.description = description;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public string GetTitle()
    {
        return title;
    }

    public string GetDescription()
    {
        return description;
    }

    public DamageData ApplyBehaviours(DamageData damageData, EnemyHealth enemy)
    {
        foreach (var behaviour in behaviours)
        {
            damageData = behaviour.ApplyBehaviour(damageData, enemy);
        }
        return damageData;
    }

    public void OnSelect()
    {
        Destroy(gameObject);
    }
}
