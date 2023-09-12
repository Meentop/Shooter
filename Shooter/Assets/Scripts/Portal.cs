using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, ISelectableItem
{
    public SelectableItems ItemType => SelectableItems.Portal;

    public string Text => "Press E to teleport";

    public void OnSelect(Player player)
    {
        player.GameManager.LoadNextFloor();
    }
}
