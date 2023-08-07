using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToRunDoor : MonoBehaviour, ISelectableItem
{
    public SelectableItems ItemType => SelectableItems.StartRun;

    public string Text => "Press E to start run";

    public void OnSelect(Player player)
    {
        
    }
}
