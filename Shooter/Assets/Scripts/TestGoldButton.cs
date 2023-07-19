using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGoldButton : MonoBehaviour, ISelectableItem
{
    [SerializeField] private int addGold;

    public SelectableItems ItemType => SelectableItems.GoldButton;

    public void OnSelect(Player player)
    {
        player.Gold.Add(addGold);
    }
}
