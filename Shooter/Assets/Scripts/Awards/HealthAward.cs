using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAward : MonoBehaviour, ISelectableItem
{
    [SerializeField] private int addHealth, price;
    private bool _wasUsed;

    public SelectableItems ItemType => SelectableItems.HealthAward;

    public string Text => _wasUsed ? "" : "Press E to buy health";

    public void OnSelect(Player player)
    {
        if (!_wasUsed && player.Gold.HasCount(price))
        {
            player.Health.AddHealth(addHealth);
            player.Gold.Remove(price);
            _wasUsed = true;
        }
    }

    public int GetAddHealth() => addHealth; 
    public int GetPrice() => price; 
    public bool IsUsed() => _wasUsed; 
}
