using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static Weapon;

public abstract class ActiveSkill : MonoBehaviour, ISelectableItem
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private int damageToReload;
    [SerializeField] private int price;

    public SelectableItems ItemType => SelectableItems.ActiveSkill;

    private float curDamageTimer;

    [System.Serializable]
    public struct Info
    {
        public Image Image;
        public Text Title;
        public Text Description;
        public Text DamageToReload;
        public Text Price;
    }

    public abstract void OnActivated();

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

    public int GetDamagaeToReturn() 
    { 
        return damageToReload; 
    }

    public int GetPrice()
    {
        return price;
    }

    public void OnSelect(Player player)
    {

    }
}
