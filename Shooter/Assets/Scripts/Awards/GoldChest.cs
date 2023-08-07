using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldChest : MonoBehaviour, ISelectableItem
{
    [SerializeField] private ActionBase dropPullableItems;
    private bool _wasUsed;

    public SelectableItems ItemType => SelectableItems.GoldAward;

    public string Text => "Press E to open";

    public void OnSelect(Player player)
    {
        if (!_wasUsed)
        {
            dropPullableItems.ExecuteAction();
            _wasUsed = true;
        }
    }
}
