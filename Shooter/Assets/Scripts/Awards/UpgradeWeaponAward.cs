using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class UpgradeWeaponAward : MonoBehaviour, ISelectableItem
{
    private bool _wasUsed;

    public SelectableItems ItemType => SelectableItems.UpgradeWeapon;

    public void OnSelect(Player player)
    {
        if (!_wasUsed && player.Gold.HasCount(player.GetSelectedWeapon().GetUpgradePrice()) && player.GetSelectedWeapon().CouldBeUpgraded())
        {
            _wasUsed = true;
        }
    }

    public bool IsUsed() => _wasUsed;
}
