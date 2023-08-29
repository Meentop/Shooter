using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class WeaponUpgradeAward : MonoBehaviour, ISelectableItem
{
    private bool _wasUsed;

    public SelectableItems ItemType => SelectableItems.WeaponUpgrade;

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
