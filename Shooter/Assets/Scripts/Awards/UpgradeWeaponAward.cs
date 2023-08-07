using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class UpgradeWeaponAward : MonoBehaviour, ISelectableItem
{
    private bool _wasUsed;

    public SelectableItems ItemType => SelectableItems.UpgradeWeapon;

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
