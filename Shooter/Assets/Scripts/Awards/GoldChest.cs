using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldChest : MonoBehaviour, ISelectableItem
{
    [SerializeField] private ActionBase dropPullableItems;

    public SelectableItems ItemType => SelectableItems.GoldAward;

    public void OnSelect()
    {
        dropPullableItems.ExecuteAction();
    }
}
