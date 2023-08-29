using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Module : MonoBehaviour, ISelectableItem
{
    [SerializeField] protected int number;
    [SerializeField] private Sprite sprite;
    [SerializeField] private string title, description;
    [SerializeField] private int price;
    public bool undestroyable = false;

    public SelectableItems ItemType => SelectableItems.Module;

    public string Text => "Press E to buy";
    public int Level { get; private set; } = 0;
    protected const int maxLevel = 2;


    [System.Serializable]
    public struct Info
    {
        public Text Type;
        public Image Image;
        public Text Title;
        public Text Description;
        public Text Price;
    }

    public Sprite GetSprite() => sprite;
    public string GetTitle(int lvl) => title + " " + (lvl + 1).ToString() + " lvl";
    public abstract string GetDescription(int lvl);
    public int GetPrice() => price;
    public bool CouldBeUpgraded() => Level < maxLevel;

    public void UpgradeModule()
    {
        Level++;
    }

    public void SetLevel(int lvl)
    {
        Level = lvl;
    }

    public void OnSelect(Player player)
    {
        
    }
}
