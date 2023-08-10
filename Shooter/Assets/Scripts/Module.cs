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
    protected int level;
    

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
    public string GetTitle() => title;
    public string GetDescription() => description;
    public int GetPrice() => price;

    public void OnSelect(Player player)
    {
        
    }
}
