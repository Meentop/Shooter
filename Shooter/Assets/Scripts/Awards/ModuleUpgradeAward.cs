using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleUpgradeAward : MonoBehaviour, ISelectableItem
{
    private bool _wasUsed;

    public SelectableItems ItemType => SelectableItems.ModuleUpgrade;

    public string Text => _wasUsed ? "" : "Press E to buy upgrade";

    public void OnSelect(Player player)
    {

    }

    public void SetWasUsed()
    {
        _wasUsed = true;
    }

    public bool IsUsed() => _wasUsed;
}
