using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGoldButton : MonoBehaviour, ISelectableItem
{
    [SerializeField] private int addGold;

    public SelectableItems ItemType => SelectableItems.GoldButton;

    public string Text => "E";

    public void OnSelect(Player player)
    {
        player.Gold.Add(addGold);
    }
}
