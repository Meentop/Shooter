using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAward : MonoBehaviour, ISelectableItem
{
    [SerializeField] private int addHealth, price;
    private bool _wasUsed;

    public SelectableItems ItemType => SelectableItems.HealthAward;

    public void OnSelect(Player player)
    {
        if (!_wasUsed && player.gold.HasCount(price))
        {
            player.health.AddHealth(addHealth);
            player.gold.Remove(price);
            _wasUsed = true;
        }
    }
}
