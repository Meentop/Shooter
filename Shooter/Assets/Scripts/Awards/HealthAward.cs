using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAward : MonoBehaviour, ISelectableItem
{
    [SerializeField] private int addHealth;
    private bool _wasUsed;

    public SelectableItems ItemType => SelectableItems.HealthAward;

    public string Text => _wasUsed ? "" : "Press E to buy health";

    public void OnSelect(Player player)
    {
        
    }

    public int GetAddHealth() => addHealth; 
    public bool IsUsed() => _wasUsed; 
    public void SetWasUsed() => _wasUsed = true;
}
